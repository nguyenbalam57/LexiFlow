import sys
import os
import logging
from flask import Flask, request, jsonify
from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer
import torch
from pathlib import Path
import signal
import atexit

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[
        logging.FileHandler('m2m100_service.log'),
        logging.StreamHandler(sys.stdout)
    ]
)

app = Flask(__name__)

class M2M100Service:
    def __init__(self):
        self.model = None
        self.tokenizer = None
        self.is_loaded = False
        self.device = "cuda" if torch.cuda.is_available() else "cpu"
        logging.info(f"Using device: {self.device}")
        self.load_model()
    
    def load_model(self):
        try:
            logging.info("Loading M2M-100 418M model...")
            
            # Create cache directory
            cache_dir = Path("./model_cache")
            cache_dir.mkdir(exist_ok=True)
            
            # Load tokenizer and model
            self.tokenizer = M2M100Tokenizer.from_pretrained(
                "facebook/m2m100_418M",
                cache_dir=str(cache_dir)
            )
            
            self.model = M2M100ForConditionalGeneration.from_pretrained(
                "facebook/m2m100_418M",
                cache_dir=str(cache_dir)
            ).to(self.device)
            
            self.model.eval()
            self.is_loaded = True
            
            logging.info("Model loaded successfully")
            
        except Exception as e:
            logging.error(f"Failed to load model: {str(e)}")
            self.is_loaded = False
            raise

# Global service instance
service = M2M100Service()

# Supported languages
SUPPORTED_LANGUAGES = {
    'en': 'English', 'vi': 'Vietnamese', 'zh': 'Chinese', 'ja': 'Japanese',
    'ko': 'Korean', 'fr': 'French', 'de': 'German', 'es': 'Spanish',
    'it': 'Italian', 'ru': 'Russian', 'ar': 'Arabic', 'hi': 'Hindi',
    'th': 'Thai', 'pt': 'Portuguese', 'nl': 'Dutch', 'pl': 'Polish'
}

@app.route('/translate', methods=['POST'])
def translate():
    if not service.is_loaded:
        return jsonify({
            'success': False,
            'error': 'Model not loaded'
        }), 503
    
    try:
        data = request.json or {}
        text = data.get('text', '').strip()
        source_lang = data.get('source_lang', 'en')
        target_lang = data.get('target_lang', 'vi')
        
        # Validation
        if not text:
            return jsonify({
                'success': False,
                'error': 'Text is required'
            }), 400
            
        if len(text) > 1000:
            return jsonify({
                'success': False,
                'error': 'Text too long (max 1000 characters)'
            }), 400
            
        if source_lang not in SUPPORTED_LANGUAGES or target_lang not in SUPPORTED_LANGUAGES:
            return jsonify({
                'success': False,
                'error': 'Unsupported language'
            }), 400
        
        # Set source language
        service.tokenizer.src_lang = source_lang
        
        # Tokenize
        encoded = service.tokenizer(
            text,
            return_tensors="pt",
            max_length=512,
            truncation=True,
            padding=True
        ).to(service.device)
        
        # Generate translation
        with torch.no_grad():
            generated_tokens = service.model.generate(
                **encoded,
                forced_bos_token_id=service.tokenizer.get_lang_id(target_lang),
                max_length=512,
                num_beams=3,
                early_stopping=True,
                do_sample=False,
                pad_token_id=service.tokenizer.pad_token_id
            )
        
        # Decode result
        result = service.tokenizer.batch_decode(
            generated_tokens,
            skip_special_tokens=True
        )[0]
        
        return jsonify({
            'success': True,
            'translated_text': result,
            'source_lang': source_lang,
            'target_lang': target_lang,
            'original_text': text
        })
        
    except Exception as e:
        logging.error(f"Translation error: {str(e)}")
        return jsonify({
            'success': False,
            'error': f'Translation failed: {str(e)}'
        }), 500

@app.route('/health', methods=['GET'])
def health_check():
    return jsonify({
        'status': 'healthy' if service.is_loaded else 'loading',
        'model': 'facebook/m2m100_418M',
        'model_loaded': service.is_loaded,
        'device': service.device,
        'languages_supported': len(SUPPORTED_LANGUAGES)
    })

@app.route('/languages', methods=['GET'])
def get_languages():
    return jsonify(SUPPORTED_LANGUAGES)

def cleanup():
    logging.info("Shutting down M2M-100 service...")
    if service.model:
        del service.model
    if service.tokenizer:
        del service.tokenizer
    torch.cuda.empty_cache() if torch.cuda.is_available() else None

# Register cleanup handlers
atexit.register(cleanup)
signal.signal(signal.SIGINT, lambda s, f: sys.exit(0))
signal.signal(signal.SIGTERM, lambda s, f: sys.exit(0))

if __name__ == '__main__':
    try:
        logging.info("Starting M2M-100 Translation Service...")
        app.run(
            host='127.0.0.1',
            port=5001,
            debug=False,
            threaded=True,
            use_reloader=False
        )
    except KeyboardInterrupt:
        logging.info("Service interrupted by user")
    except Exception as e:
        logging.error(f"Service error: {str(e)}")
    finally:
        cleanup()
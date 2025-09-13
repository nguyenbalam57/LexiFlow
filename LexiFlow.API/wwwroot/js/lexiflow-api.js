/**
 * LexiFlow API JavaScript Enhancement
 * Provides additional functionality for the API documentation
 */

(function() {
    'use strict';

    // LexiFlow API Configuration
    const LexiFlowAPI = {
        version: '1.0.0',
        name: 'LexiFlow API',
        description: 'Japanese Language Learning Platform API',
        
        // Initialize when DOM is ready
        init() {
            console.log(`🌸 ${this.name} v${this.version} - Enhanced UI Loaded`);
            this.setupEventHandlers();
            this.enhanceSwaggerUI();
            this.addCustomFeatures();
        },
        
        // Setup event handlers
        setupEventHandlers() {
            // Add click tracking for API endpoints
            document.addEventListener('click', (e) => {
                if (e.target.matches('.swagger-ui .opblock-summary')) {
                    const method = e.target.querySelector('.opblock-summary-method')?.textContent;
                    const path = e.target.querySelector('.opblock-summary-path')?.textContent;
                    console.log(`API Endpoint clicked: ${method} ${path}`);
                }
            });
            
            // Track authentication events
            document.addEventListener('click', (e) => {
                if (e.target.matches('.btn.authorize')) {
                    console.log('Authentication dialog opened');
                }
            });
        },
        
        // Enhance Swagger UI with custom features
        enhanceSwaggerUI() {
            // Wait for Swagger UI to load
            setTimeout(() => {
                this.addVersionInfo();
                this.improveResponsiveness();
                this.addKeyboardShortcuts();
            }, 1000);
        },
        
        // Add version information
        addVersionInfo() {
            const infoSection = document.querySelector('.swagger-ui .info');
            if (infoSection && !document.querySelector('.lexiflow-version-info')) {
                const versionDiv = document.createElement('div');
                versionDiv.className = 'lexiflow-version-info';
                versionDiv.style.cssText = `
                    background: linear-gradient(45deg, #e3f2fd, #f3e5f5);
                    padding: 15px;
                    border-radius: 8px;
                    margin: 15px 0;
                    border-left: 4px solid #3498db;
                `;
                versionDiv.innerHTML = `
                    <h4 style="color: #2c3e50; margin-bottom: 10px;">🚀 API Information</h4>
                    <p><strong>Version:</strong> ${this.version}</p>
                    <p><strong>Environment:</strong> ${this.getEnvironment()}</p>
                    <p><strong>Base URL:</strong> ${window.location.origin}</p>
                    <p><strong>Documentation:</strong> Enhanced with LexiFlow custom features</p>
                `;
                infoSection.appendChild(versionDiv);
            }
        },
        
        // Improve responsiveness
        improveResponsiveness() {
            // Add viewport meta if not present
            if (!document.querySelector('meta[name="viewport"]')) {
                const viewport = document.createElement('meta');
                viewport.name = 'viewport';
                viewport.content = 'width=device-width, initial-scale=1.0';
                document.head.appendChild(viewport);
            }
            
            // Improve mobile experience
            const style = document.createElement('style');
            style.textContent = `
                @media (max-width: 768px) {
                    .swagger-ui .wrapper {
                        padding: 0 10px !important;
                    }
                    
                    .swagger-ui .opblock .opblock-summary {
                        padding: 10px 15px !important;
                    }
                    
                    .swagger-ui .info {
                        margin: 20px 0 !important;
                    }
                    
                    .lexiflow-version-info {
                        font-size: 0.9em !important;
                        padding: 10px !important;
                    }
                }
            `;
            document.head.appendChild(style);
        },
        
        // Add keyboard shortcuts
        addKeyboardShortcuts() {
            document.addEventListener('keydown', (e) => {
                // Ctrl/Cmd + K to focus search (if available)
                if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
                    e.preventDefault();
                    const searchInput = document.querySelector('.swagger-ui input[type="text"]');
                    if (searchInput) {
                        searchInput.focus();
                        console.log('🔍 Search focused via keyboard shortcut');
                    }
                }
                
                // Ctrl/Cmd + / to show shortcuts help
                if ((e.ctrlKey || e.metaKey) && e.key === '/') {
                    e.preventDefault();
                    this.showShortcutsHelp();
                }
            });
        },
        
        // Show shortcuts help
        showShortcutsHelp() {
            if (document.querySelector('.lexiflow-shortcuts-modal')) return;
            
            const modal = document.createElement('div');
            modal.className = 'lexiflow-shortcuts-modal';
            modal.style.cssText = `
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0,0,0,0.7);
                display: flex;
                align-items: center;
                justify-content: center;
                z-index: 10000;
            `;
            
            modal.innerHTML = `
                <div style="
                    background: white;
                    padding: 30px;
                    border-radius: 12px;
                    max-width: 400px;
                    width: 90%;
                    box-shadow: 0 25px 50px rgba(0,0,0,0.3);
                ">
                    <h3 style="color: #2c3e50; margin-bottom: 20px;">⌨️ Keyboard Shortcuts</h3>
                    <ul style="list-style: none; padding: 0; line-height: 2;">
                        <li><kbd>Ctrl/Cmd + K</kbd> - Focus search</li>
                        <li><kbd>Ctrl/Cmd + /</kbd> - Show this help</li>
                        <li><kbd>Escape</kbd> - Close modals</li>
                    </ul>
                    <button onclick="this.closest('.lexiflow-shortcuts-modal').remove()" 
                            style="
                                background: #3498db;
                                color: white;
                                border: none;
                                padding: 10px 20px;
                                border-radius: 5px;
                                cursor: pointer;
                                margin-top: 15px;
                                float: right;
                            ">Close</button>
                </div>
            `;
            
            // Close on backdrop click
            modal.addEventListener('click', (e) => {
                if (e.target === modal) {
                    modal.remove();
                }
            });
            
            // Close on Escape key
            document.addEventListener('keydown', function escapeHandler(e) {
                if (e.key === 'Escape') {
                    modal.remove();
                    document.removeEventListener('keydown', escapeHandler);
                }
            });
            
            document.body.appendChild(modal);
        },
        
        // Get current environment
        getEnvironment() {
            const hostname = window.location.hostname;
            if (hostname === 'localhost' || hostname === '127.0.0.1') {
                return 'Development';
            } else if (hostname.includes('staging') || hostname.includes('test')) {
                return 'Staging';
            } else {
                return 'Production';
            }
        },
        
        // Add custom features
        addCustomFeatures() {
            // Add loading indicator for API calls
            this.setupLoadingIndicators();
            
            // Add response time tracking
            this.trackResponseTimes();
            
            // Add copy-to-clipboard for code examples
            this.addCopyFeatures();
        },
        
        // Setup loading indicators
        setupLoadingIndicators() {
            // Monitor fetch requests to show loading states
            const originalFetch = window.fetch;
            window.fetch = function(...args) {
                console.log('🔄 API Request initiated:', args[0]);
                return originalFetch.apply(this, args)
                    .then(response => {
                        console.log('✅ API Response received:', response.status);
                        return response;
                    })
                    .catch(error => {
                        console.error('❌ API Request failed:', error);
                        throw error;
                    });
            };
        },
        
        // Track response times
        trackResponseTimes() {
            // Simple performance tracking
            window.performance.mark('lexiflow-api-start');
            
            // Track when page is fully loaded
            window.addEventListener('load', () => {
                window.performance.mark('lexiflow-api-end');
                window.performance.measure('lexiflow-api-load', 'lexiflow-api-start', 'lexiflow-api-end');
                
                const measures = window.performance.getEntriesByName('lexiflow-api-load');
                if (measures.length > 0) {
                    console.log(`📊 Page load time: ${measures[0].duration.toFixed(2)}ms`);
                }
            });
        },
        
        // Add copy-to-clipboard functionality
        addCopyFeatures() {
            // Will be enhanced when Swagger UI loads
            setTimeout(() => {
                const codeBlocks = document.querySelectorAll('.swagger-ui pre');
                codeBlocks.forEach(block => {
                    if (!block.querySelector('.copy-button')) {
                        const copyBtn = document.createElement('button');
                        copyBtn.className = 'copy-button';
                        copyBtn.textContent = '📋';
                        copyBtn.style.cssText = `
                            position: absolute;
                            top: 5px;
                            right: 5px;
                            background: #3498db;
                            color: white;
                            border: none;
                            padding: 5px 8px;
                            border-radius: 3px;
                            cursor: pointer;
                            font-size: 12px;
                        `;
                        copyBtn.onclick = () => {
                            navigator.clipboard.writeText(block.textContent).then(() => {
                                copyBtn.textContent = '✅';
                                setTimeout(() => copyBtn.textContent = '📋', 2000);
                            });
                        };
                        
                        block.style.position = 'relative';
                        block.appendChild(copyBtn);
                    }
                });
            }, 2000);
        }
    };

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => LexiFlowAPI.init());
    } else {
        LexiFlowAPI.init();
    }

    // Expose API for external use
    window.LexiFlowAPI = LexiFlowAPI;

})();
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Core.Validation
{
    /// <summary>
    /// Base class for entity validators
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class EntityValidator<T> where T : BaseEntity
    {
        private readonly ILogger _logger;

        protected EntityValidator(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Validates an entity
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Validation result</returns>
        public ValidationResult Validate(T entity)
        {
            try
            {
                // Perform basic validation using data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(entity);

                bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

                // Perform custom validation
                var customResults = ValidateCustomRules(entity);

                // Combine results
                validationResults.AddRange(customResults);

                return new ValidationResult
                {
                    IsValid = isValid && !customResults.Any(),
                    Errors = validationResults
                        .Select(r => new ValidationError
                        {
                            PropertyName = r.MemberNames.FirstOrDefault() ?? string.Empty,
                            ErrorMessage = r.ErrorMessage ?? "Validation error"
                        })
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating entity {EntityType}", typeof(T).Name);

                return new ValidationResult
                {
                    IsValid = false,
                    Errors = new List<ValidationError>
                    {
                        new ValidationError
                        {
                            PropertyName = string.Empty,
                            ErrorMessage = $"Validation error: {ex.Message}"
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Validates custom business rules
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Validation results</returns>
        protected abstract IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> ValidateCustomRules(T entity);
    }

    /// <summary>
    /// Validator for User entities
    /// </summary>
    public class UserValidator : EntityValidator<User>
    {
        public UserValidator(ILogger<UserValidator> logger) : base(logger)
        {
        }

        protected override IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> ValidateCustomRules(User entity)
        {
            var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Username must be at least 3 characters
            if (entity.Username.Length < 3)
            {
                errors.Add(new System.ComponentModel.DataAnnotations.ValidationResult(
                    "Username must be at least 3 characters long",
                    new[] { nameof(entity.Username) }));
            }

            // Username must not contain spaces
            if (entity.Username.Contains(' '))
            {
                errors.Add(new System.ComponentModel.DataAnnotations.ValidationResult(
                    "Username must not contain spaces",
                    new[] { nameof(entity.Username) }));
            }

            // Email must be valid
            if (!IsValidEmail(entity.Email))
            {
                errors.Add(new System.ComponentModel.DataAnnotations.ValidationResult(
                    "Email is not valid",
                    new[] { nameof(entity.Email) }));
            }

            return errors;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Validator for VocabularyItem entities
    /// </summary>
    public class VocabularyItemValidator : EntityValidator<VocabularyItem>
    {
        public VocabularyItemValidator(ILogger<VocabularyItemValidator> logger) : base(logger)
        {
        }

        protected override IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> ValidateCustomRules(VocabularyItem entity)
        {
            var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Term must not be empty
            if (string.IsNullOrWhiteSpace(entity.Term))
            {
                errors.Add(new System.ComponentModel.DataAnnotations.ValidationResult(
                    "Term must not be empty",
                    new[] { nameof(entity.Term) }));
            }

            // Language code must be valid
            if (!IsValidLanguageCode(entity.LanguageCode))
            {
                errors.Add(new System.ComponentModel.DataAnnotations.ValidationResult(
                    "Language code is not valid",
                    new[] { nameof(entity.LanguageCode) }));
            }

            // Difficulty level must be between 1 and 5
            if (entity.DifficultyLevel < 1 || entity.DifficultyLevel > 5)
            {
                errors.Add(new System.ComponentModel.DataAnnotations.ValidationResult(
                    "Difficulty level must be between 1 and 5",
                    new[] { nameof(entity.DifficultyLevel) }));
            }

            return errors;
        }

        private bool IsValidLanguageCode(string languageCode)
        {
            // Simple validation for language codes (2-letter or 5-letter with hyphen)
            return !string.IsNullOrEmpty(languageCode) &&
                   (languageCode.Length == 2 ||
                    (languageCode.Length == 5 && languageCode[2] == '-'));
        }
    }

    /// <summary>
    /// Factory for creating validators
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Gets a validator for the specified entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Entity validator</returns>
        EntityValidator<T> GetValidator<T>() where T : BaseEntity;
    }

    /// <summary>
    /// Factory implementation for creating validators
    /// </summary>
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidatorFactory> _logger;

        public ValidatorFactory(IServiceProvider serviceProvider, ILogger<ValidatorFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public EntityValidator<T> GetValidator<T>() where T : BaseEntity
        {
            try
            {
                // Try to get a registered validator
                var validator = _serviceProvider.GetService(typeof(EntityValidator<T>)) as EntityValidator<T>;

                if (validator != null)
                {
                    return validator;
                }

                // Log warning
                _logger.LogWarning("No validator found for entity type {EntityType}", typeof(T).Name);

                // Return a default validator that always validates successfully
                return new DefaultValidator<T>(_logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting validator for entity type {EntityType}", typeof(T).Name);

                // Return a default validator that always validates successfully
                return new DefaultValidator<T>(_logger);
            }
        }

        /// <summary>
        /// Default validator that always validates successfully
        /// </summary>
        private class DefaultValidator<T> : EntityValidator<T> where T : BaseEntity
        {
            public DefaultValidator(ILogger logger) : base(logger)
            {
            }

            protected override IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> ValidateCustomRules(T entity)
            {
                return Enumerable.Empty<System.ComponentModel.DataAnnotations.ValidationResult>();
            }
        }
    }

    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Flag indicating if validation was successful
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        /// <summary>
        /// Gets error messages for a specific property
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Error messages</returns>
        public IEnumerable<string> GetErrorMessages(string propertyName)
        {
            return Errors
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage);
        }

        /// <summary>
        /// Gets all error messages
        /// </summary>
        /// <returns>Error messages</returns>
        public IEnumerable<string> GetAllErrorMessages()
        {
            return Errors.Select(e => e.ErrorMessage);
        }

        /// <summary>
        /// Gets a formatted error message
        /// </summary>
        /// <returns>Formatted error message</returns>
        public string GetFormattedErrorMessage()
        {
            if (IsValid)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, GetAllErrorMessages());
        }
    }

    /// <summary>
    /// Validation error
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Property name
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
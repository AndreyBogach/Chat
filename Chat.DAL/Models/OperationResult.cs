using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Models
{
    public class OperationResult
    {
        private readonly List<string> _errors = new List<string>();

        public OperationResult() { }

        protected OperationResult(bool success, string errorMessage)
        {
            Success = success;
            if (!success)
            {
                AddError(errorMessage);
            }
            Data = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Data { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Errors
        {
            get { return _errors.ToArray(); }
            set
            {
                foreach (var r in value)
                    _errors.Add(r);
            }
        }

        public string ErrorsCombined
        {
            get
            {
                if (_errors.Count == 0) return null;
                if (_errors.Count == 1) return _errors[0];

                return "Operation failed with the following errors:" +
                    Environment.NewLine +
                    _errors.Aggregate((previous, current) => previous + Environment.NewLine + current);
            }
        }

        public static OperationResult CreateWithSuccess() { return new OperationResult(true, null); }

        public static OperationResult CreateWithError(string errorMessage)
        {
            return new OperationResult(false, errorMessage);
        }

        public void AddError(string message)
        {
            Success = false;
            _errors.Add(message);
        }

        public void AddErrors(IEnumerable<string> messages)
        {
            Success = false;
            _errors.AddRange(messages);
        }

        public override string ToString()
        {
            if (Success) return "Success!";

            if (_errors.Count == 1) return _errors[0];

            return _errors.Count + " errors: " + _errors.Aggregate((current, next) => current + " " + next);
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }

        public OperationResult() { } //for deserialization

        protected OperationResult(bool success, T value, string errorMessage)
            : base(success, errorMessage)
        {
            if (success) Value = value;
        }

        public static OperationResult<T> CreateWithSuccess(T value) { return new OperationResult<T>(true, value, null); }

        public static OperationResult<T> CreateWithError(string errorMessage)
        {
            return new OperationResult<T>(false, default(T), errorMessage);
        }
    }
}
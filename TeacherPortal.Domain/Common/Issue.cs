using System.ComponentModel;

namespace TeacherPortal.Domain.Common
{
    public class Issue
    {
        private IList<string> _description;
        public IssueCode Code { get; }
        public IList<string> Description
        {
            get => _description;
        }

        public Issue(IssueCode code, params string[] description)
        {
            Code = code;
            _description = description?.ToList() ?? new List<string>();
        }

        public void AddDescriptions(params string[] descriptions)
        {
            UnionDescriptions(descriptions);
        }

        public void AddDescriptions(IList<string> descriptions)
        {
            UnionDescriptions(descriptions);
        }

        private void UnionDescriptions(IEnumerable<string> descriptions)
        {
            _description = _description.Union(descriptions, StringComparer.InvariantCultureIgnoreCase).ToList();
        }


    }
    public enum IssueCode
    {
        [Description("UNAUTHORISED_REQUEST")]
        UNAUTHORISED_REQUEST = 0,
        [Description("VALIDATION_ERROR")]
        VALIDATION_ERROR = 1,

    }

}

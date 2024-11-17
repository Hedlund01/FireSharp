using FireSharp.Model;

namespace FireSharp.Data;

public class AdminPermissions
{
    private const string _prefix = "AdminPermissions";
    
    public class Roles: IPermission
    {
        private const string _groupPrefix = $"{_prefix}.Roles";
        public const string  Read = $"{_groupPrefix}.Read";
        public  const string Write = $"{_groupPrefix}.Write";
        public  const string Delete = $"{_groupPrefix}.Delete";
    }
    
    public class Menu: IPermission
    {
        private const string _groupPrefix = $"{_prefix}.Menu";
        public const string  Read = $"{_groupPrefix}.Read";
    }
}
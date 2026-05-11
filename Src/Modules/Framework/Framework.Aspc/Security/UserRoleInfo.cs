using Framework.Aspc.Security;
using Framework.Security;
using Infrastructure.Security;

namespace Infrastructure.Security
{
    public class UserRoleInfo
  {
    public UserRoleInfo(UserInfo userinfo)
    {
      UserInfo = userinfo;
      IsAuthenticate = userinfo.IsAuthenticated;
    }

    private UserInfo UserInfo { get; }

    public bool IsAuthenticate { get; private set; } = false;

    public bool IsAzadUniversityEmployee
    {
      get
      {
        return IsInRole(RoleNameConst.AzadUniversityEmployee);
      }
    }
        
    public bool IsAzadUniversityFaculty
    {
      get
      {
        return IsInRole(RoleNameConst.AzadUniversityFaculty);
      }
    }
        
    public bool IsAzadUniversityStudent
    {
      get
      {
        return IsInRole(RoleNameConst.AzadUniversityStudent);
      }
    }
        
    public bool IsAdmin
    {
      get
      {
        return IsInRole(RoleNameConst.Admin);
      }
    }
    public bool IsParticipant
    {
      get
      {
        return IsInRole(RoleNameConst.Participant);
      }
    }
    public bool IsStudent
    {
      get
      {
        return IsInRole(RoleNameConst.Student);
      }
    }
    public bool IsUniversityStudent
    {
      get
      {
        return IsInRole(RoleNameConst.UniversityStudent);
      }
    }
    public bool IsScientificSecretary
    {
      get
      {
        return IsInRole(RoleNameConst.ScientificSecretary);
      }
    }
    public bool IsCustomer
    {
      get
      {
        return IsInRole(RoleNameConst.Customer);
      }
    }
    public bool IsExecutiveSecretary
    {
      get
      {
        return IsInRole(RoleNameConst.ExecutiveSecretary);
      }
    }
    public bool IsEducationConnector
    {
      get
      {
        return IsInRole(RoleNameConst.EducationConnector);
      }
    }
    public bool IsFinancialConnector
    {
      get
      {
        return IsInRole(RoleNameConst.FinancialConnector);
      }
    }
    public bool IsTeacher
    {
      get
      {
        return IsInRole(RoleNameConst.Teacher);
      }
    }
    public bool IsSecretaryOfTheArbitrationCommittee
    {
      get
      {
        return IsInRole(RoleNameConst.SecretaryOfTheArbitrationCommittee);
      }
    }
    public bool IsReferee
    {
      get
      {
        return IsInRole(RoleNameConst.Referee);
      }
    }
    public bool IsResearcher
    {
      get
      {
        return IsInRole(RoleNameConst.Researcher);
      }
    }
    public bool IsSuperAdmin
    {
      get
      {
        return IsInRole(RoleNameConst.SuperAdmin);
      }
    }
    public bool SuperAdminFinancial
    {
            get
            {
                return IsInRole(RoleNameConst.SuperAdminFinancial);
            }
    } 
        
    private bool IsInRole(string roleName)
    {
      if (IsAuthenticate is false)
      {
        return false;
      }
      if (string.IsNullOrEmpty(roleName))
      {
        return false;
      }

      var result =      UserInfo.Role?.Where(x => x == roleName).FirstOrDefault();
      if(result == null) return false;
      return true;
    }
  }
}

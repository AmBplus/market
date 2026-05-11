using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.NumberHelper;
using Framework.Resources;
namespace Framework.Security
{
    public class RoleEnumeration : Enumeration
    {
        public static RoleEnumeration SuperAdmin = new RoleEnumeration(RoleKeyNameConst.SuperAdmin, RoleName.SuperAdmin);
        public static RoleEnumeration Admin = new RoleEnumeration(RoleKeyNameConst.Admin, RoleName.Admin);
        public static RoleEnumeration ExecutiveSecretary = new RoleEnumeration(RoleKeyNameConst.ExecutiveSecretary, RoleName.ExecutiveSecretary);
        public static RoleEnumeration AzadUniversityEmployee = new RoleEnumeration(RoleKeyNameConst.AzadUniversityEmployee, RoleName.AzadUniversityEmployee);
        public static RoleEnumeration AzadUniversityFaculty = new RoleEnumeration(RoleKeyNameConst.AzadUniversityFaculty, RoleName.AzadUniversityFaculty);
        public static RoleEnumeration AzadUniversityStudent = new RoleEnumeration(RoleKeyNameConst.Student, RoleName.Student);
        public static RoleEnumeration EducationalExpert = new RoleEnumeration(RoleKeyNameConst.EducationalExpert, RoleName.EducationalExpert);
        public static RoleEnumeration Expert = new RoleEnumeration(RoleKeyNameConst.Expert, RoleName.Expert);
        public static RoleEnumeration ScientificSecretary = new RoleEnumeration(RoleKeyNameConst.ScientificSecretary, RoleName.ScientificSecretary);
        public static RoleEnumeration EducationConnector = new RoleEnumeration(RoleKeyNameConst.EducationConnector, RoleName.EducationConnector);
        public static RoleEnumeration ResearchConnector = new RoleEnumeration(RoleKeyNameConst.ResearchConnector, RoleName.ResearchConnector);
        public static RoleEnumeration FinancialConnector = new RoleEnumeration(RoleKeyNameConst.FinancialConnector, RoleName.FinancialConnector);
        public static RoleEnumeration SuperAdminFinancial = new RoleEnumeration(RoleKeyNameConst.SuperAdminFinancial, RoleName.SuperAdminFinancial);
        public static RoleEnumeration SecretaryOfTheArbitrationCommittee = new RoleEnumeration(RoleKeyNameConst.SecretaryOfTheArbitrationCommittee, RoleName.SecretaryOfTheArbitrationCommittee);
        
        public static RoleEnumeration Researcher = new RoleEnumeration(RoleKeyNameConst.Researcher, RoleName.Researcher);
        public static RoleEnumeration Referee = new RoleEnumeration(RoleKeyNameConst.Referee, RoleName.Referee);
        
        public static RoleEnumeration Participant = new RoleEnumeration(RoleKeyNameConst.Participant, RoleName.Participant);
        public static RoleEnumeration UniversityStudent = new RoleEnumeration(RoleKeyNameConst.UniversityStudent, RoleName.UniversityStudent);
        public static RoleEnumeration Student = new RoleEnumeration(RoleKeyNameConst.Student, RoleName.Student);
        public static RoleEnumeration Teacher = new RoleEnumeration(RoleKeyNameConst.Teacher, RoleName.Teacher);
        public static RoleEnumeration Customer = new RoleEnumeration(RoleKeyNameConst.Customer, RoleName.Customer);

        private RoleEnumeration(long id, string name) : base(id, name)
        {
        }
        public RoleEnumeration() { }
    }
}

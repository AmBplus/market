namespace Web.Helper
{
    public class ApplicationConstant
    {
        public class RequestTeaching
        {
            public const long MaxSampleVideoFileSize = 157286400;
        }
        public class Teacher
        {
            public const long MaxCourseUploadFile = 157286400;
        }
        public class ProjectFileSize
        {
            public const long MaxAllowedSizeFile = 157286401;
            public const long MaxAllowedSizeFileByProject = 550 * 1024 * 1024;
        }
        public class ProProfile
        {
            public const long MaxNationalCardImageSize = 1048576;
            public const long MaxDeggreeSize = 1048576;
            public const long MaxResumeSize = 5242880;

        }
        public class Profile
        {
            public const long MaxPicProfile = 512000;


        }
        public class Course
        {
            public const long MaxPicCourse = 512000;

        }

        public class Domain
        {
            public const long MaxPicDomain = 512000;

        }
    }
}

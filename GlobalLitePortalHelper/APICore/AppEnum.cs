

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GlobalLitePortalHelper.APICore
{
    public enum RetirementType
    {
        Exact = 1,
        Claim,
        Refund
    }
    public enum RetireMode
    {
        Cash = 1,
        BankTransfer,
        Cheque,
        E_Payment,
    }
    public enum DisbursementStatus
    {
        Disbursed = 1,
        Awaiting_Retirement, 
        Closed
    }
    public enum ExpenseStatus
    {
        Awaiting_Disbursement = 1,
        Disbursing,
        Disbursed,
        Delivered,
        Closed,
        Void
    }
    public enum DisburseMode
    {
        Cash = 1,
        Bank_Transfer,
        Cheque,
        E_Payment 
    }

    public enum TaskStatus
    {
        Registered = 1,
        Pending,
        Viewed, 
        Done, 
        Suspended,
        Cancelled,
        Closed, 
    }

    public enum WorkflowStage
    {
        Initiation =1,// "Initiation",
        Approvals ,//= "Approvals",
        Disbursement ,//= "Disbursement",
        Retirement ,//= "Receipt/Retirement",
        Closed ,//= "Closed"
    }
    public enum RequestItemStatus
    {
    Deleted = -100,
    Registered = 1,
    Workflow_Processing,
    Completed,
    Closed,

    }

    public enum WorkflowStatus
    {
    Processing = 1,
    Approved,
    Denied,
    Queried,

    }

    public enum RequestStatus
    {
    Deleted = -100,
    Denied = -1,
    Queried = -2,
    Suspended = 0,
    Registered = 1,
    Ratification,
    Approved,
    Processing,
    Disbursed,
    Completed,
    Closed

    }
    public enum BeneficiaryType
    {
        Staff = 1,
        Vendor,
        Entity
    }

    public enum RequestFrequencyType
    {
        Daily = 1,
        Weekly = 7,
        Monthly = 30,
        Annualy = 365,
    }
    public enum RequestType
    {
        [Display(Name = "Capital Expense")]
        Capital = 1,
        [Display(Name = "Petty Expense")]
        Petty,
        [Display(Name = "Statutory Expense")]
        Statutory,
        [Display(Name ="Budgetory Expense")]
        Budgetory,
        [Display(Name ="Inner Company Expense")]
        Inner_Company,
        [Display(Name ="Salary Wages Expense")]
        Salary_Wages,
        [Display(Name ="Others Expense")]
        Others,
    }

    public enum ExpenseLookupItems
    {
        ExpenseCategory = 1,
        ExpenseItem,
        ExpenseClassification,
        ExpenseType,
        WorkflowRole,
        WorkflowStage,
        WorkflowApprovalLevel,
        Department,
    }

   

    public enum LookupItem

    {

        ClassOfCertificate = 1,

        Country,

        CourseOfStudy,

        Discipline,

        Institution,

        LocalArea,

        OLevelExamBody,

        OLevelGrade,

        OLevelSubject,

        ProfessionalBodyAward,

        ProfessionalBody,

        Qualification,

        QualificationClassOfCertificate,

        State,

        ExperienceDuration,

        ExamLocation,

        ExamCenter

    }

    public enum AppSourceType

    {

        Android = 1,

        IOS,

        Web,

        Windows

    }

    public enum RuleItem

    {

        Nationality_Required = 1,

        State_Of_Origin_Required = 2,

        Local_Area_Of_Origin_Required = 3,

        Country_Of_Residence_Required = 4,

        State_Of_Residence_Required = 5,

        Local_Area_Of_Residence_Required = 6,

        Disability_Required = 7,

        Town_Of_Residence_Required = 8,

        Residential_Address_Required = 9,



        Date_Of_Birth_Benchmark = 21,

        Minimum_Age = 22,

        Maximum_Age = 23,



        Primary_Education_Required = 35,

        Secondary_Education_Required = 36,

        OLevel_English_Required = 37,

        OLevel_Maths_Required = 38,

        OLevel_Total_Sitting = 39,

        Min_OLevel_Subjects = 40,

        Max_OLevel_Subjects = 41,

        Min_OLevel_Grade = 42,



        Minimum_Higher_Qualification = 51,

        Maximum_Higher_Qualification = 52,

        Minimum_Class_Of_Certificate = 53,

        Maximum_Class_Of_Certificate = 54,

        Maximum_Higher_Education_Items = 55,



        Min_Professional_Qualification = 65,

        Max_Professional_Qualification = 66,

        Min_Award = 64,

        Max_Award = 67,

        Min_Volunteer = 68,

        Max_Volunteer = 69,



        NYSC_Required = 70,

        NYSC_Exemption_Allowed = 71,



        Minimum_Years_Work_Experience = 80,

        Maximum_Years_Work_Experience = 81,

        Min_No_Of_Work_Experience = 82,

        Max_No_Of_Work_Experience = 83,

        Job_Position_Required = 84,

        Job_Level_Required = 85,

        Job_Specialization_Required = 86,

    }

    public enum RuleType

    {

        Personal_Info = 1,

        Junior_Education,

        Higher_Education,

        Professional_Certification,

        NYSC_Info,

        Work_Experience,

    }

    public enum CurrentImplementation { Sandbox = 1, Live = 2 }


    public enum SessionCurrentStage { None = 0, Reg_Started, Reg_Paused, Reg_Closed, Shortlist_Stage, Exam_Stage, Essay_Stage, Selection_Stage, Interview_Stage, Completed }

    public enum InstantTestType { Single_Question_Area = 1, All_Question_Area }

    public enum SessionControlType { Start = 1, Pause, Resume, Restart, End }

}

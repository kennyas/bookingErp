using System;
using Tornado.Shared.Enums;

namespace UserManagement.Core.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string LastLoginDate { get; set; }
        public string GenderTitle { get; set; }
        public int Gender { get; set; }
        public string MiddleName { get; set; }
        public Guid Id { get; internal set; }
        public string Role { get; set; }
        public string DialingCode { get;  set; }
    }

    public class BusBoyViewModel: UserViewModel {
        public int Status { get; set; }
    }

    public class CaptainViewModel:UserViewModel {
        public int Status { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class StaffViewModel : UserViewModel
    {
        public string Unit { get; set; }
        public string Department { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class PartnerViewModel : UserViewModel
    {
        public string PartnerPhoneNumber { get; set; }
        public string PartnerAddress { get; set; }
        public string PartnerEmail { get; set; }
        public Guid PartnerId { get; set; }
    }

    public class UserViewDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public Gender Gender { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }
        public Guid Id { get; set; }
        public string DialingCode { get; set; }
    }

    public class BusBoyViewDto : UserViewDto
    {
        public int Status { get; set; }
    }

    public class CaptainViewDto: UserViewDto
    {
        public int Status { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class StaffListViewDto : UserViewDto
    {
        public int TotalCount { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class PartnerViewDto : UserViewDto
    {   
        public string PartnerPhoneNumber { get; set; }
        public string PartnerAddress { get; set; }
        public string PartnerEmail { get; set; }
        public Guid PartnerId { get; set; }
    }


    public class UserListViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string LastLoginDate { get; set; }
    }

    public class StaffListViewModel : UserListViewModel
    {
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
    }

    public class PartnerListViewDto : UserViewDto
    {
        public string PartnerPhoneNumber { get; set; }
        public string PartnerAddress { get; set; }
        public string PartnerEmail { get; set; }
        public int TotalCount { get; set; }
    }

    public class CaptainListViewModel : UserListViewModel
    {
        public string EmployeeCode { get; set; }
    }

    public class CustomerListViewModel : UserListViewModel
    {
    }

    public class BusBoyListViewModel : UserListViewModel
    {
    }

    public class PartnerListViewModel : UserListViewModel
    {

    }
}
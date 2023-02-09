namespace Online_Book_Store_Application.Utility
{
    public static class StaticDetails
    {
        //user
        public const string Role_User_Individual = "Individual";
        public const string Role_User_Company = "Company";
        //management
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

		//orderStatus
		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "InProcess";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Refunded";

		//paymentStatus
		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejected = "Rejected";
	}
}
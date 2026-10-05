class CustomerAccount {
  const CustomerAccount({
    required this.name,
    required this.email,
    required this.companyName,
  });

  final String name;
  final String email;
  final String companyName;

  factory CustomerAccount.fromJson(Map<String, dynamic> json) {
    return CustomerAccount(
      name: '${json['customerContactName'] ?? json['CustomerContactName'] ?? ''}',
      email: '${json['primaryContactEmail'] ?? json['PrimaryContactEmail'] ?? ''}',
      companyName: '${json['facilityName'] ?? json['FacilityName'] ?? ''}',
    );
  }
}
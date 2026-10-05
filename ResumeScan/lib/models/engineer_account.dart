class EngineerAccount {
  const EngineerAccount({
    required this.id,
    required this.name,
    required this.email,
  });

  final int id;
  final String name;
  final String email;

  factory EngineerAccount.fromJson(Map<String, dynamic> json) {
    return EngineerAccount(
      id: (json['fieldServiceEngineerId'] ??
              json['FieldServiceEngineerId'] ??
              0)
          as int,
      name: '${json['engineerName'] ?? json['EngineerName'] ?? ''}',
      email: '${json['emailAddress'] ?? json['EmailAddress'] ?? ''}',
    );
  }
}
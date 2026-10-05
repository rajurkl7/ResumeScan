class StateOption {
  const StateOption({
    required this.id,
    required this.name,
    required this.isEnabled,
  });

  final int id;
  final String name;
  final bool isEnabled;

  factory StateOption.fromJson(Map<String, dynamic> json) {
    return StateOption(
      id: (json['stateId'] ?? json['StateId']) as int,
      name: '${json['stateName'] ?? json['StateName'] ?? ''}',
      isEnabled: (json['isEnabled'] ?? json['IsEnabled'] ?? false) as bool,
    );
  }
}

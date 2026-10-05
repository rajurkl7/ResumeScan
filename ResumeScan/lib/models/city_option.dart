class CityOption {
  const CityOption({
    required this.id,
    required this.stateId,
    required this.stateName,
    required this.name,
    required this.isEnabled,
  });

  final int id;
  final int stateId;
  final String stateName;
  final String name;
  final bool isEnabled;

  factory CityOption.fromJson(Map<String, dynamic> json) {
    return CityOption(
      id: (json['cityId'] ?? json['CityId']) as int,
      stateId: (json['stateId'] ?? json['StateId']) as int,
      stateName: '${json['stateName'] ?? json['StateName'] ?? ''}',
      name: '${json['cityName'] ?? json['CityName'] ?? ''}',
      isEnabled: (json['isEnabled'] ?? json['IsEnabled'] ?? false) as bool,
    );
  }
}

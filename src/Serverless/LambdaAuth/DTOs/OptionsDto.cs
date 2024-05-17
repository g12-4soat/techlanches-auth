namespace TechLanchesLambda.DTOs;

public record class OptionsDto(string Region = "", string UserPoolId = "", string UserPoolClientId = "", string UserTechLanches = "", string EmailDefault = "", string PasswordDefault = "");

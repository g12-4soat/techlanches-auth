namespace TechLanchesLambda.DTOs;

public record class OptionsDto(string Region = "us-east-1", string UserPoolId = "", string UserPoolClientId = "", string UserTechLanches = "", string EmailDefault = "", string PasswordDefault = "");

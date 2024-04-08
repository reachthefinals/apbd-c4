using System;
using Xunit; 

namespace LegacyApp.test;

public class UserUtilTests
{
    [Fact]
    public void UserUtil_RelativeAge_Test1()
    {
        int age1 = LegacyApp.UserUtil.RelativeAge(DateTime.ParseExact("07/04/2018", "dd/MM/yyyy", null));
        int age2 = LegacyApp.UserUtil.RelativeAge(DateTime.ParseExact("19/05/2018", "dd/MM/yyyy", null));
        Assert.Equal(6, age1);
        Assert.Equal(5, age2);
    }

    [Fact]
    public void UserUtil_ValidateEmail_Test1()
    {
        Assert.True(UserUtil.IsValidMail("abc@awd.xyz"));
        Assert.True(UserUtil.IsValidMail("test@gmail.com"));
        Assert.False(UserUtil.IsValidMail("xdd@awdawd"));
        Assert.False(UserUtil.IsValidMail("awd,gmail.com"));
        Assert.False(UserUtil.IsValidMail("awd@gmail"));
        Assert.False(UserUtil.IsValidMail("@gmail.com"));
    }
}
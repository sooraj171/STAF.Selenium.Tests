# UI test template

```csharp
[TestMethod]
public void ScenarioName_DescribesIntent()
{
  NavigateTo(TestContext.Properties["purl"].ToString());
  new Login(driver, TestContext)
      .LoginToApplication(
          TestContext.Properties["userName"].ToString(),
          TestContext.Properties["password"].ToString())
      .VerifyAccountsOverviewPageisLoaded();
}
```

Invalid login (stays on `Login` action):

```csharp
new Login(driver, TestContext)
    .LoginToApplicationInvalid("bad", "bad")
    .VerifyInvalidUserMessageIsDisplayed();
```

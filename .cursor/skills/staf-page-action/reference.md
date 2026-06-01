# Page + action templates

**Page (minimal):**

```csharp
public class FooPage : PageBaseClass
{
    #region ObjectIdentifierValues
    private string _btnSubmit = "submit";
    #endregion

    public FooPage(IWebDriver driver, TestContext ctx) : base(driver, ctx) { }

    public IWebElement btnSubmit => FindAppElement(By.Id(_btnSubmit));
}
```

**Action (verification):**

```csharp
public class Foo : FooPage
{
    public Foo(IWebDriver driver, TestContext ctx) : base(driver, ctx) { }

    public Foo VerifyFooPageLoaded()
    {
        btnSubmit.ReportElementIsDisplayed(Driver, context, nameof(VerifyFooPageLoaded), "Foo loaded", false);
        return this;
    }
}
```

**Action (flow returning next screen):**

```csharp
public Bar ClickGoToBar()
{
    Click(btnSubmit); // or page-specific click helper on base/wrappers
    ReportResult.ReportResultPass(Driver, context, nameof(ClickGoToBar), "Navigated");
    return new Bar(Driver, context);
}
```

Use existing `EnterUserName` / `Click` patterns from `Login.cs` where applicable.

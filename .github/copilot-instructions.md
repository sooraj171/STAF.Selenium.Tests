# Project instructions for GitHub Copilot (VS Code)

When editing or generating code in this repo, follow these STAF Selenium framework rules.

---

## 1. Framework

- All tests must inherit from **TestBaseClass** (UI) or **TestBaseAPI** (API/Excel/DB).
- Driver must **never** be instantiated directly in tests or pages; use the base class driver.
- All waits must use **framework wait helpers** (e.g. FindAppElement, WaitForDocumentReady); **no raw Thread.Sleep**.
- Page objects must follow the project pattern: inherit **PageBaseClass**, use **FindAppElement(By)** / **FindAppElement(parent, By, description)**.
- This prevents generic Selenium code and keeps reporting and cleanup consistent.

## 2. Tool usage (MCP / code generation)

- When navigating → use **NavigateTo(url)** (or framework equivalent), not raw driver.Navigate().
- When clicking → use **Click(locator)** or page/action methods that wrap it.
- When typing → use **EnterText(locator, text)** or page/action methods.
- When validating → use the **framework assertion wrapper** (e.g. ReportElement, ReportElementIsDisplayed).
- When handling dropdowns → use **framework utility**, not raw Select(driver.FindElement(...)).
- Generated code must use the project’s abstraction layer; do not bypass it.

## 3. Test creation workflow

1. **Identify page** – Determine which page/screen the test needs.
2. **Check if Page Object exists** – Look in Pages/ (e.g. LoginPage, AboutUsPage).
3. **If not, create Page Object** – Use PageBaseClass, add locators and FindAppElement-based methods.
4. **Add locators** in the **centralized location** (page class or shared locators as per project).
5. **Add reusable methods** on the page/action classes (no raw driver in tests).
6. **Write test** using **page/action methods only**; test class inherits TestBaseClass.
7. **Add assertions** using the **framework wrapper** (ReportResult, ReportElement).

## 4. Coding standards

- Use **explicit locators only** (By.Id, By.CssSelector, By.XPath only when necessary).
- Prefer **data-testid** or stable attributes when available.
- **Avoid brittle XPath** (e.g. long positional paths); prefer ID, data-testid, or short CSS.
- Use **meaningful test names** that describe scenario or intent.
- Follow **AAA (Arrange–Act–Assert)** in test methods.
- **Do not duplicate** existing page/action methods; reuse and extend.

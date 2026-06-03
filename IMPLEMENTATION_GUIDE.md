# Implementation Guide — Using the New Setup

Step-by-step instructions for using the new cross-platform setup in each IDE.

---

## 🔵 Visual Studio Implementation

### Prerequisites

- Visual Studio 2022+ with GitHub Copilot extension
- Signed into GitHub account
- Project open: `STAF.Selenium.Tests`

### Setup (One-Time)

1. **Install GitHub Copilot**
   - Extensions → Manage Extensions
   - Search "GitHub Copilot"
   - Install if not present
   - Reload VS

2. **Sign In**
   - `Help` → `GitHub Copilot` → `Sign In`
   - Complete authentication
   - Ready to use

3. **Custom agents (Visual Studio 2026 18.4+)**
   - Repo agents live in `.github/agents/`:
     - **STAF UI Automation** — UI tests, pages, actions
     - **STAF API Automation** — REST tests, requests, DTOs
   - In Copilot Chat (agent mode), open the **agent picker** or type `@staf-ui-automation` / `@staf-api-automation`
   - Repo instructions still load from `.github/copilot-instructions.md`

### Using Copilot for Code Generation

#### Task 1: Create a New Test Method

```
1. Open: STAFTests/Tests/MyNewTests.cs (or any test file)
2. Press: Ctrl+Shift+I (GitHub Copilot Chat)
3. Type:
   "Create a new UI test method in this file
	Test: LoginToApp_InvalidCredentials_ErrorDisplayed
	Navigate to Parabank login page
	Log in with invalid credentials ('bad', 'bad')
	Verify error message is displayed

	Use the pattern from @STAFTests/Tests/ParaTests.cs
	and @STAFTests/Actions/Login.cs
	Inherit TestBaseClass, call action methods only"

4. Copilot generates code in chat window
5. Click "Insert" or copy/paste into your file
6. Verify: dotnet build
7. Run: dotnet test --filter "ClassName~MyNewTests"
```

#### Task 2: Create a Page + Action

```
1. Create new files (or open existing):
   - STAFTests/Pages/MyNewPage.cs
   - STAFTests/Actions/MyNew.cs

2. In Pages/MyNewPage.cs, press Ctrl+Shift+I
3. Type:
   "Create a page object class for MyNewPage
	Inherit PageBaseClass
	Elements:
	- Submit button: id='submit'
	- Email input: name='email'
	- Error message: css='.error'

	Use the pattern from @STAFTests/Pages/LoginPage.cs
	Include #region ObjectIdentifierValues
	All properties use FindAppElement with descriptions"

4. Verify code, then create Action in MyNew.cs
5. In Actions/MyNew.cs, press Ctrl+Shift+I
6. Type:
   "Create an action class MyNew inheriting MyNewPage
	Include methods:
	- VerifyPageLoaded(): check submit button displayed
	- EnterEmail(string email): enter email, return this
	- ClickSubmit(): click button, return NextPage

	Use the pattern from @STAFTests/Actions/Login.cs
	Every method has ReportResult calls
	Use nameof() for method names"

7. Verify code
8. Update docs/ai-index.json: pwsh tools/UpdateAiIndex.ps1
```

#### Task 3: Create an API Test

```
1. Open: STAFTests/Requests/CreateRequests.cs (Ctrl+Shift+I)
2. Type:
   "Add a method GetGitHubUser(string username)
	Call https://api.github.com/users/{username}
	Return RestResponse<GitHubUserDTO>

	Use async/await, RestSharp v114
	Pattern from existing methods in this file"

3. Then open: STAFTests/APIData/GitHubUserDTO.cs (Ctrl+Shift+I)
4. Type:
   "Create a DTO class GitHubUserDTO
	Map JSON response: id, login, name, avatar_url, public_repos
	Use [JsonPropertyName] for field mapping"

5. Then open: STAFTests/Tests/APITests.cs (Ctrl+Shift+I)
6. Type:
   "Add test method GetGitHubUser_ValidUsername_ReturnsSuccess
	Inherit TestBaseAPI
	Call GetGitHubUser('torvalds')
	Check status 200
	Assert data not null
	Use ReportResultAPI for reporting
	Pattern from existing tests in this file"
```

### Keyboard Shortcuts (VS)

| Action | Shortcut |
|--------|----------|
| Open Copilot Chat | `Ctrl+Shift+I` |
| Insert selected suggestion | `Tab` |
| Reject suggestion | `Esc` |
| Go to Definition | `F12` |
| Find References | `Shift+F12` |
| Format Document | `Shift+Alt+F` |
| Command Palette | `Ctrl+Shift+P` |

### Configuration

VS automatically loads from:
- `.github/copilot-instructions.md` ← All quick rules, patterns
- `docs/AI_GUIDE.md` ← Referenced for deep dives

**No additional setup needed** — just start asking Copilot!

### Tips

- ✅ **Reference specific files:** `@STAFTests/Actions/Login.cs`
- ✅ **Ask step-by-step:** One task per prompt
- ✅ **Request checklists:** "Verify against the checklist in docs/AI_GUIDE.md"
- ✅ **Check golden files:** Always open them first for reference
- ❌ **Don't:** Paste entire error logs (summarize instead)

---

## 🟦 VS Code Implementation

### Prerequisites

- VS Code installed
- C# extension installed (`ms-dotnettools.csharp`)
- GitHub Copilot extension installed
- Workspace folder: `STAF.Selenium.Tests`

### Setup (First Time)

1. **Install Extensions**
   ```
   Ctrl+Shift+X (Extensions)
   Search: "GitHub Copilot"
   Click Install

   Search: "C# Dev Kit"
   Click Install

   Reload VS Code
   ```

2. **Sign Into GitHub**
   ```
   Ctrl+Shift+P (Command Palette)
   Type: "GitHub Copilot: Sign In"
   Complete browser authentication
   ```

3. **Verify Setup**
   ```
   Open any .cs file
   Ctrl+Shift+I (should open Copilot Chat)
   Ask: "What framework is this project using?"
   Should respond about STAF.UI.API
   ```

### Using Copilot Chat

#### Basic Workflow

```
1. Ctrl+Shift+I (Open Copilot Chat)
2. Type your question
3. Reference files with @filename: @STAFTests/Actions/Login.cs
4. Reference docs with #filename: #docs/AI_GUIDE.md
5. Review generated code in chat
6. Click "Insert" or copy/paste
7. Review and adjust as needed
```

#### Example: Create UI Test

```
Ctrl+Shift+I

"Create a UI test for invalid login
 File: STAFTests/Tests/MyTests.cs

 Reference these files for pattern:
 @STAFTests/Tests/ParaTests.cs
 @STAFTests/Actions/Login.cs

 Inherit TestBaseClass
 Navigate to Parabank
 Call action: LoginToApplicationInvalid('bad', 'bad')
 Verify error message displayed

 Check docs/AI_GUIDE.md#workflow-1-ui-test for full details"
```

#### Example: Create Page + Action

```
Ctrl+Shift+I

"Create page and action for new screen

Page (STAFTests/Pages/MyNewPage.cs):
- Inherit PageBaseClass
- Button: id='submit'
- Input: name='email'
- Use #region ObjectIdentifierValues
- All properties return FindAppElement

Action (STAFTests/Actions/MyNew.cs):
- Inherit MyNewPage
- Method: VerifyPageLoaded()
- Method: EnterEmail(string email)
- Method: ClickSubmit() → returns NextPage
- Every method uses ReportResult

Reference: @STAFTests/Pages/LoginPage.cs
		   @STAFTests/Actions/Login.cs"
```

### Keyboard Shortcuts (VS Code)

| Action | Shortcut |
|--------|----------|
| Open Copilot Chat | `Ctrl+Shift+I` |
| Open Command Palette | `Ctrl+Shift+P` |
| Go to Definition | `F12` |
| Find References | `Shift+F12` |
| Format Document | `Shift+Alt+F` |
| Open Terminal | `Ctrl+`` |

### Configuration Files

VS Code automatically uses:
- `.vscode/settings.json` ← Test settings
- `.vscode/README.md` ← This IDE's setup guide
- `.github/copilot-instructions.md` ← Rules & patterns
- `docs/AI_GUIDE.md` ← Master guide (reference)

### Inline Suggestions (Optional)

For inline code completion while typing:

```csharp
public void Test[Copilot suggests: MyTest_Scenario_Expected]
// Press Tab to accept, Esc to reject
```

### Testing from VS Code

```powershell
# Open Terminal: Ctrl+`

# Run single test
dotnet test --filter "FullyQualifiedName~STAFTests.MyTests.MyTest_Scenario_Expected" `
	--settings STAFTests/testrunsetting.runsettings

# Build
dotnet build STAFTests/STAF.Selenium.Tests.csproj

# Run all tests
dotnet test --settings STAFTests/testrunsetting.runsettings
```

### Tips for VS Code

- ✅ **File references:** `@STAFTests/Pages/LoginPage.cs` (auto-complete shows files)
- ✅ **Doc references:** `#docs/AI_GUIDE.md#workflow-1-ui-test` (specific section)
- ✅ **Keep chat focused:** One question at a time
- ✅ **Copy from golden files:** Open them side-by-side for reference
- ❌ **Don't:** Ask too many things in one prompt

---

## 🟪 Cursor Implementation

### Prerequisites

- Cursor editor installed (https://cursor.sh)
- Workspace open: `STAF.Selenium.Tests`
- Internet connection (for AI features)

### Setup (First Time)

1. **Verify Skills Loaded**
   ```
   Cmd+K (open Composer/Command)
   Type: "What skills are available?"
   Should list: staf-ui-test, staf-page-action, staf-api-test
   ```

2. **Verify Rules Loaded**
   ```
   Cmd+K
   Type: "What rules apply to code generation?"
   Should reference .cursor/cursor.rules
   ```

3. **Test a Simple Generation**
   ```
   Cmd+K
   Type: "staf-ui-test: Describe the UI test workflow"
   Should explain 6-step workflow
   ```

### Using Skills in Cursor

#### Skill-Triggered Generation

```
1. Cmd+K (Composer)
2. Type: "{skill-name}: {task description}"
3. Cursor triggers the skill automatically
4. Review generated code
5. Click "Accept" or edit and save
```

#### Examples

**UI Test:**
```
Cmd+K

staf-ui-test: Create a test for invalid login
 - Class: MyTests.cs
 - Method: LoginToApp_InvalidCredentials_ErrorMessage
 - Navigate to Parabank
 - Login with bad/bad
 - Verify error message appears
```

**Page + Action:**
```
Cmd+K

staf-page-action: Create a page and action for contact form screen
 - Page: ContactFormPage.cs with name, email, message fields
 - Action: ContactForm.cs with methods to fill form and submit
 - Return next page after submit
 - Include VerifyPageLoaded() and all ReportResult calls
```

**API Test:**
```
Cmd+K

staf-api-test: Create API test for GitHub users endpoint
 - Request method: GetGitHubUsers(int page)
 - DTO: GitHubUsersListDTO
 - Test: GetGitHubUsers_Page1_ReturnsSuccess
 - Check status, assert data, use ReportResultAPI
```

### Composer vs Chat vs Cmd+K

| Feature | Composer | Chat | Cmd+K |
|---------|----------|------|-------|
| **Generate Code** | ✅ Best | ✅ Yes | ✅ Yes |
| **Question Q&A** | ✅ Yes | ✅ Better | ⚠️ Limited |
| **Multi-file** | ✅ Best | ✅ Yes | ❌ Single |
| **Keyboard** | `Cmd+I` | `Cmd+L` | `Cmd+K` |

**Recommendation:** Use `Cmd+K` for quick generations, `Cmd+I` (Composer) for multi-file work.

### Rules Enforcement

Cursor automatically applies `.cursor/cursor.rules`:

```
❌ Violations (Cursor will flag):
- Creating new IWebDriver()
- Using Thread.Sleep()
- Missing ReportResult calls
- Wrong inheritance (TestBaseAPI in UI test)

✅ Cursor will suggest:
- Use inherited driver property
- Use FindAppElement instead
- Add ReportResult call
- Change to TestBaseClass
```

### Keyboard Shortcuts (Cursor)

| Action | Shortcut |
|--------|----------|
| Composer (generate) | `Cmd+I` |
| Chat | `Cmd+L` |
| Command Palette | `Cmd+K` |
| Go to Definition | `Cmd+Click` |
| Find References | `Shift+F12` |
| Format Document | `Shift+Alt+F` |

### Configuration

Cursor automatically reads:
- `.cursor/cursor.rules` ← Consistency rules
- `.cursor/skills/MASTER.md` ← Skill index
- `.cursor/skills/*/SKILL.md` ← Individual skills
- `docs/AI_GUIDE.md` ← Full patterns (referenced by skills)

**No setup needed** — Cursor auto-discovers!

### Tips for Cursor

- ✅ **Reference skills by name:** "staf-page-action: Create..."
- ✅ **Ask Cursor to verify:** "Before finishing, check against .cursor/cursor.rules"
- ✅ **Use Composer for complex:** Multi-file generations
- ✅ **Keep rules in view:** Open `.cursor/cursor.rules` while generating
- ❌ **Don't:** Ask Cursor to ignore rules — they're there for good reason

---

## Troubleshooting by Platform

### Visual Studio

| Problem | Solution |
|---------|----------|
| **Copilot Chat won't open** | `Ctrl+Shift+P` → "GitHub Copilot: Sign In" |
| **Generated code has wrong inheritance** | Reference golden file: "Use LoginPage.cs pattern" |
| **Too much code generated at once** | Ask step-by-step: "First just the Page..." |
| **Doesn't know STAF patterns** | Paste `.github/copilot-instructions.md` quick rules into prompt |

### VS Code

| Problem | Solution |
|---------|----------|
| **No Copilot Chat** | `Ctrl+Shift+X` → Install GitHub Copilot extension |
| **Can't reference files** | Use `@filename` syntax; VS Code will autocomplete |
| **Generated code misses XML comments** | Ask: "Add /// <summary> comments to all public methods" |
| **Test won't compile** | Check `using` statements; ask: "Add missing using directives" |

### Cursor

| Problem | Solution |
|---------|----------|
| **Skills not showing** | Restart Cursor; check `.cursor/skills/` folder exists |
| **Rules not enforced** | Open `.cursor/cursor.rules` in editor to verify syntax |
| **Generated code violates rules** | Use Composer (`Cmd+I`) instead of Cmd+K for better context |
| **Wrong file created** | Specify full path: "Create in STAFTests/Pages/MyNewPage.cs" |

---

## Best Practices Across All Platforms

### Before Generating

1. ✅ Check `docs/ai-index.json` — reuse existing if possible
2. ✅ Open golden file in adjacent window — reference while generating
3. ✅ Have `docs/QUICK_START.md` open — check checklist
4. ✅ Know what files you're creating — be specific in prompt

### During Generation

1. ✅ Be specific — include file paths, method names, field names
2. ✅ Reference patterns — "Use LoginPage.cs pattern"
3. ✅ Ask for verification — "Before finishing, check against..."
4. ✅ Review code carefully — AI can make mistakes

### After Generation

1. ✅ Build: `dotnet build STAFTests/STAF.Selenium.Tests.csproj`
2. ✅ Test: `dotnet test --filter "ClassName~YourClass"`
3. ✅ Update index: `pwsh tools/UpdateAiIndex.ps1`
4. ✅ Commit: Include the new files in your PR

---

## Common Prompts (Copy & Paste)

### For Any Platform

**Create UI Test:**
```
Create a UI test in STAFTests/Tests/{Class}Tests.cs

Method: {TestName}
Workflow:
1. Navigate to TestContext.Properties["purl"]
2. Call {ActionClass} methods (reference @STAFTests/Actions/{Action}.cs)
3. Chain: navigate → interact → verify

Inherit TestBaseClass
No By.* selectors, no raw driver usage
Pattern from @STAFTests/Tests/ParaTests.cs
```

**Create Page + Action:**
```
Create page and action objects:

Page: STAFTests/Pages/{Screen}Page.cs
- Inherit PageBaseClass
- Locators: {list elements with locators}
- All properties use FindAppElement(By.*, selector, "description")

Action: STAFTests/Actions/{Screen}.cs
- Inherit {Screen}Page
- Methods: {list methods with descriptions}
- Every method returns this or new {NextScreen}(driver, context)
- ReportResult calls for all steps

Pattern from @STAFTests/Pages/LoginPage.cs and @STAFTests/Actions/Login.cs
```

**Create API Test:**
```
Create API test:

Request: Add method to CreateRequests.cs
- {MethodName}({parameters})
- Call {endpoint}
- Return RestResponse<{DTOName}>
- Use async/await

DTO: Create APIData/{DTOName}.cs
- Auto-properties for: {list fields}
- Use [JsonPropertyName] for JSON mapping

Test: Add to APITests.cs
- Method: {TestName}
- Check status code, assert content
- Use ReportResultAPI

Pattern from existing methods in CreateRequests.cs and APITests.cs
```

---

## Daily Workflow

### Morning: Setup Check (1 minute)

```
1. Open your IDE (VS / VS Code / Cursor)
2. Open docs/QUICK_START.md
3. Find your task
4. Start coding!
```

### Creating Code (5-10 minutes)

```
1. Identify: UI Test, Page+Action, or API Test
2. Open Copilot/Composer/Cmd+K
3. Paste relevant prompt from docs
4. Review generated code
5. Verify: Build + Run test
6. Commit with updated ai-index.json
```

### Stuck? (2 minutes)

```
1. Open docs/AI_GUIDE.md
2. Find "Code Patterns" section
3. Compare your code with examples
4. Ask AI to explain difference
```

---

## Next: Get Started!

1. **Pick your IDE** (VS, VS Code, or Cursor)
2. **Read that section above** (🔵, 🟦, or 🟪)
3. **Try the first example** — Copy/paste a prompt
4. **Review the checklist** from `docs/QUICK_START.md`
5. **Run your test** — See it pass!

---

**Last Updated:** 2026-05-31  
**Framework:** STAF.UI.API v4.4.0+  
**Platforms:** Visual Studio, VS Code, Cursor

# 🧩 TemplateCreator

This project is a Windows Forms application developed to easily create a **.NET Backend project architecture** based on **Onion Architecture**.  
It takes **project name**, **project location**, and **.NET version** from the user, then automatically generates the required folders and layers.

---

## 🚀 Features
- Automatically generates the base layers for **Onion Architecture**:
  - **Core**
    - Domain
    - Application
  - **Infrastructure**
    - Infrastructure
    - Persistence
  - **Presentation**
    - API
- Automatically creates a **Solution (.sln)**
- Adds projects to the **Solution**
- Defines **references** between layers
- Simple and user-friendly interface (Windows Forms)

---

## 📋 Requirements
- .NET SDK (e.g., `.NET 8.0` or `.NET 7.0`)
- Windows operating system
- Git Bash / CMD / PowerShell (to use dotnet CLI)

---

## 🔧 Usage
1. To run the application, simply extract publish.rar and run setup.exe inside it.
2. Enter the new **Project Name**.  
   _(Any spaces will be automatically removed)_
3. Select a **Project Location** folder.
4. Choose the **.NET Version** (e.g., `8.0`, `7.0`).
5. Click the **Create Project** button.
6. The application will generate the necessary layers and add the references, providing you with a ready-to-use solution.

---

## 📂 Example Output Structure
```
MyProject/
│   MyProject.sln
│
├── Core/
│   ├── MyProject.Domain/
│   └── MyProject.Application/
│
├── Infrastructure/
│   ├── MyProject.Infrastructure/
│   └── MyProject.Persistence/
│
└── Presentation/
    └── MyProject.API/
```

---

## 🛠 Layer References
- **Application → Domain**
- **Infrastructure → Application + Domain**
- **Persistence → Infrastructure + Domain**
- **API → Application + Infrastructure**

---

## 📌 Notes
- Currently, only **Onion Architecture** is supported.
- Future versions may include other architectures (Clean Architecture, Hexagonal, etc.).
- Make sure the selected `.NET version` is installed on your machine.

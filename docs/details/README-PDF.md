# STAF Framework — PDF User Guide

**STAF-Framework-Architecture-and-User-Guide.pdf** — End-user document covering architecture, features, reporting, and novelty. All architecture diagrams are embedded in the PDF.

**To regenerate the PDF** (requires Node.js):

```bash
cd docs/details/pdf-build
npm install
npm run generate-pdf
```

The PDF is generated from **[STAF-Framework-User-Guide.html](../STAF-Framework-User-Guide.html)** in the parent `docs/` folder. You can also open that HTML file in a browser and use **Print → Save as PDF** if you prefer not to use Node.js.

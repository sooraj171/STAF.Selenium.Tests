const path = require('path');
const fs = require('fs');

async function generatePdf() {
  const puppeteer = require('puppeteer');
  const htmlPath = path.resolve(__dirname, 'STAF-Framework-User-Guide.html');
  const pdfPath = path.resolve(__dirname, 'STAF-Framework-Architecture-and-User-Guide.pdf');

  if (!fs.existsSync(htmlPath)) {
    console.error('HTML file not found:', htmlPath);
    process.exit(1);
  }

  const browser = await puppeteer.launch({ headless: 'new' });
  const page = await browser.newPage();
  await page.goto('file://' + htmlPath, { waitUntil: 'networkidle0' });
  await page.waitForSelector('.mermaid svg', { timeout: 15000 }).catch(() => {});
  await new Promise(r => setTimeout(r, 1500));

  await page.pdf({
    path: pdfPath,
    format: 'A4',
    printBackground: true,
    margin: { top: '20px', right: '20px', bottom: '20px', left: '20px' }
  });

  await browser.close();
  console.log('PDF written to:', pdfPath);
}

generatePdf().catch(err => {
  console.error(err);
  process.exit(1);
});

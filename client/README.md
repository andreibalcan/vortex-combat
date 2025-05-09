# Vortex Combat Client

This file contains information about the frontend of Vortex Combat. This Angular-based application serves as the user interface for interacting with the back-end APIs, handling authentication, and all available pages.

## 🧩 Architecture
This front-end follows a standard SPA (Single Page Application) model using: 
- **Angular 19.2.4** as the frontend framework.
- **PrimeNG** as the component library for UI components.
- **RxJs** and services for reactive state and API communication.
- **JWT (JSON Web Tokens)** for authentication.

## ⚙ Requirements
Before running this project, make sure your system is set up with the required tooling.

### Node.js and npm
Angular requires Node.js and npm (Node Package Manager) to manage dependencies and run the development server.

Recommended version: **Node.js 18+**.

> Download Node.js (which includes npm) [here](https://nodejs.org/en/download).

### Angular CLI
You need the Angular CLI in order to build and serve this application.

```
npm install -g @angular/cli@19.2.10
```

## 🛠 First-Time Setup
Follow these steps to set up the project locally:

#### 1. Install project dependencies
Navigate to the `client` folder and run:

```
npm install
```

This installs all dependencies listed in `package.json`.

#### 2. Running the App
```
ng serve
```

You can then explore and test the UI interface, available at:
> http://localhost:4200/login

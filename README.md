# Vortex Combat 🥋

**Vortex Combat** is a modern martial arts gyn management platform, focusing specifically in Jiu-Jitsu. It allows masters and students to interact in a digital environment that supports the tracking training progress, belt progress, workout scheduling, certifications, and communication.

This project is divided into two main parts:

- **Front-end**: An Angular application (v19) powered by PrimeNG for UI components.
- **Back-end**: A secure, RESTful ASP.NET Core API using Entity Framework and Identity for authentication, data handling, and business logic.

## 📌 Project Goal
The project's mission is to bring structure, efficiency, and traceability to martial arts academies by offering a digital system that mirrors the discipline of physical training.

The development of this project follows the NOMIS (Normative Modelling of Information Systems) methodology, a **human-centered** approach to the
modeling and development of information systems proposed by [Dr. José Cordeiro](https://www.researchgate.net/profile/Jose-Cordeiro).

The application includes:
- Role-based access (Students and Master).
- Belt progression tracking.
- Attendance, belt and progress management.
- JWT-based authentication.

## ⚙ Architecture Summary

| Layer      | Tech Stack                                         |
|-----------:|----------------------------------------------------|
| Front-end   | Angular 19, PrimeNG, TypeScript                    |
| Back-end    | ASP.NET Core 9, Entity Framework Core, MySQL 8.4.4 |
| Auth       | ASP.NET Identity + JWT                            |
| Infra      | .env-based config, EF migrations, Swagger UI       |

For more detailed setup, configuration, and development steps:

> **[Frontend README](./client/README.md)**.

> **[Backend README](./server/README.md)**.

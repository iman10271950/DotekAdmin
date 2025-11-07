# 🐄 Dotek — Smart Livestock & Poultry Trading Platform

**Dotek** is an online platform designed to revolutionize the traditional livestock and poultry market in Iran.  
Our goal is to make the buying and selling process between **farmers, sellers, and buyers** more **secure, transparent, and fast** through technology.

---

## 🚀 Key Features

- 📦 **Seller Load Posting:** Sellers can list their available livestock or products with detailed info.  
- 💰 **Buyer Bidding System:** Buyers can submit their price offers for each listed load.  
- 🔐 **Secure Payment System:** Safe and direct payments between buyers and sellers through an internal payment process.  
- 📊 **Detailed Reports & Analytics:** Real-time sales and purchase insights for better decision-making.  
- 🐔 **Advanced Categorization:** Covers livestock, poultry, feed, equipment, and raw materials.  

---

## 🧠 Vision

Dotek aims to digitize the traditional livestock and poultry market by merging **technology, trust, and transparency**.  
We want to create a platform that:
- Guarantees **cash payments** for sellers.  
- Ensures **direct profit** for buyers through middleman-free transactions.  
- Builds **trust and clarity** in every deal.

---

## 🧩 Technical Stack

| Layer | Description |
|-------|--------------|
| **Frontend** | React.js + TailwindCSS |
| **Backend** | ASP.NET Core (Clean Architecture + CQRS + EF Core) |
| **Database** | SQL Server |
| **Caching & Messaging** | Redis + RabbitMQ |
| **DevOps** | Docker + IIS Deployment |
| **Version Control** | Git + GitHub Actions |

---

## 🧱 Architecture

The project follows **Clean Architecture** principles for modularity, scalability, and testability:


Dotek/
│
├── Dotek.API → API layer (for frontend communication)
├── Dotek.Core → Domain models, business logic, and services
├── Dotek.Infrastructure → EF Core, Redis, RabbitMQ, and external services
└── Dotek.Tests → Unit and integration tests


---

## ⚙️ Getting Started (Development)

1. **Clone the repository:**
   ```bash
   git clone https://github.com/iman10271950/dotek.git
   cd dotek


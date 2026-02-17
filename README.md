# Adrenaline Rush

3D racing game developed in Unity/C#, featuring an **Object-Oriented Programming** architecture, AI agents with spatial awareness, physics-driven vehicle handling, and a persistent upgrade economy.
**The Vision**: Uniting the concept of a high-speed arcade racer with a realistic sim racer.

<p align="center">
  <img src="https://github.com/user-attachments/assets/55454899-40b0-4b43-9d62-5d1a4c0ce96d" width="100%" alt="Adrenaline Rush Hero Video"/>
</p>

---

## Key Features

### Intelligent AI Pathfinding With Avoidance

* **Dynamic Pathfinding:** AI utilise a custom waypoint system and random steering deviations for human-like track navigation.
* **Spatial Awareness:** Implemented real-time **Raycasting** for obstacle avoidance, allowing AI to detect opponents and steer to avoid collisions.
* **Recovery Logic:** Integrated "Still-Detection" and teleportation systems to ensure AI agents recover from crashes automatically.
* **Adaptive Difficulty:** Dynamic scaling of AI motor force and steering deviations based on user-selected difficulty level (Easy, Medium, Hard).

> **[PLACEHOLDER: AI_Avoidance.gif]**

### Physics-Based Handling

* **Dynamic Steering:** Steering angles are calculated via **Linear Interpolation (Lerp)**, reducing sensitivity at high speeds to simulate realistic vehicle weight and stability.
* **Suspension & Friction:** Fully modelled suspension via `WheelColliders` for realistic weight transfer during corners and drifting.
* **Procedural Audio:** Engine audio pitch is dynamically adjusted in real-time based on the vehicle's linear velocity.

> **[PLACEHOLDER: Physics_Handling.gif]**

### Tactical Power-Ups & Combat

* **Enum-Based System:** A modular power-up architecture handling randomised power-ups including **Missiles**, **EMP Blasts**, and **Speed Boosts**.
* **Combat Disruptors:** Missiles and EMPs physically impact AI opponents, freezing their recovery logic to provide a tactical advantage.
* **Feedback System:** High-impact visual feedback through world-space UI and active projectile tracking.

> **[PLACEHOLDER: Combat_System.gif]**

### Upgrades & Progression

* **Garage System:** A persistent shop where players can buy and equip new vehicles with unique paint jobs using earned race currency.
* **Persistent Economy:** Rewards and race history (date, position, time) are serialized via **JSON** and saved using **PlayerPrefs**.
* **Global Configuration:** Persistent user settings for Audio Mixer volumes, graphics quality, and dynamic UI scaling.

> **[PLACEHOLDER: Garage_Economy.gif]**

---

## Technical Architecture (OOP Focus)

This project emphasizes clean, modular C# code following core OOP principles:

* **Encapsulation:** Controlled data access in `GarageManager.cs`, `OptionsManager.cs`, and `RaceProgress.cs`, protecting player state and preferences from external corruption.
* **Abstraction:** The `PosManager.cs` simplifies complex race ranking math into a single "Position" value for the UI, while `AIController.cs` hides complex navigation math from the engine.
* **Polymorphic Logic:** Utilized **Enums and Switch-case structures** in `PowerUpManager.cs` to manage diverse combat behaviors through a unified activation interface.
* **Decoupled Systems:** Heavy use of specialized managers for Power-Ups, Race state, Camera views, and UI to ensure a high degree of modularity.

---

## Project Structure

* **`Assets/Scripts/`**: Core C# logic including `AIController`, `CarController`, `RaceManager`, and `PosManager`.
* **`Packages/`**: Essential manifest files defining project dependencies.
* **`ProjectSettings/`**: Global configurations, including custom tags (`AI`, `Car`, `BRAKE`) and Input System bindings.

---

## How to Run Locally

1. **Clone the repo:**

```bash
git clone https://github.com/mahraz-ahmed/adrenaline-rush.git

```

2. **Open in Unity:** Ensure you are using **Unity 6000.0.25f1**.
3. **Git LFS:** This project uses Git LFS for high-resolution textures. Ensure LFS is installed to view full art assets.
4. **Load Scene:** Open `Assets/Scenes/Main Menu.unity`.
5. **Press Play.**

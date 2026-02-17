# Adrenaline Rush

3D racing game developed in Unity/C#, featuring an **Object-Oriented Programming** architecture, AI agents with spatial awareness, physics-driven vehicle handling, and a persistent upgrade economy.

**The Vision**: Uniting the concept of a high-speed arcade racer with a realistic sim racer.

<p align="center">
  <img src="https://github.com/user-attachments/assets/78eaabae-a717-4368-bbee-f08181a83df5" width="90%" alt="Adrenaline Rush Hero Video"/>
</p>


## Key Features

### Intelligent AI Pathfinding With Avoidance

* **Dynamic Pathfinding:** AI utilise a custom waypoint system and random steering deviations for human-like track navigation.
* **Spatial Awareness:** Implemented real-time **Raycasting** for obstacle avoidance, allowing AI to detect opponents and steer to avoid collisions.
* **Recovery Logic:** Integrated "Still-Detection" and teleportation systems to ensure AI agents recover from crashes automatically.
* **Adaptive Difficulty:** Dynamic scaling of AI motor force and steering deviations based on user-selected difficulty level (Easy, Medium, Hard).

<p align="center">
  <img src="https://github.com/user-attachments/assets/cfc60dca-72d3-4061-ad1d-29fa465c9f40" width="90%" alt="AI Demo"/>
</p>

### Advanced Game Management

* **Race Manager:** Supervises and coordinates the full game loop from the difficulty selection, to the countdown, to the race termination and awards, ensuring game integrity.
* **Total Progress Algorithm:** Calculates precise vehicle ranking by factoring in total completed laps, current waypoint index, and normalised distance to the next node.
* **Dynamic Standings Sorting:** Utilises C# LINQ to sort all active vehicles by total progress, ensuring the HUD position counter is accurate during high-speed, neck-and-neck racing.

<p align="center">
  <img src="https://github.com/user-attachments/assets/5997ff96-a23e-4561-962a-0a8a7524a8ac" width="90%" alt="Countdown"/>
</p>

### Tactical Power-Ups & Combat

* **Enum-Based System:** A modular power-up architecture handling randomised power-ups including **Missiles**, **EMP Blasts**, and **Speed Boosts**.
* **Combat Disruptors:** Missiles and EMPs physically impact AI opponents, freezing their recovery logic to provide a tactical advantage.
* **Feedback System:** High-impact visual feedback through world-space UI and active projectile tracking.

<p align="center">
  <img src="https://github.com/user-attachments/assets/07bd9518-2647-4637-a20d-b12f993e3efe" width="90%" alt="Missile"/>
</p>

### Upgrades & Progression

* **Garage System:** A persistent shop where players can buy and equip new vehicles with unique paint jobs using earned race currency.
* **Persistent Economy:** Rewards and race history (date, position, time) are serialised via **JSON** and saved using **PlayerPrefs**.
* **Global Configuration:** Persistent user settings for Audio Mixer volumes, graphics quality, and dynamic UI scaling.

<p align="center">
  <img src="https://github.com/user-attachments/assets/23760fe2-da08-4fd6-b371-dafa4775c717" width="90%" alt="Progression"/>
</p>


## Technical Architecture (OOP Focus)

This project emphasizes clean, modular C# code following core OOP principles:

* **Encapsulation:** Controlled data access in `GarageManager.cs`, `OptionsManager.cs`, and `RaceProgress.cs`, protecting player state and preferences from external corruption.
* **Abstraction:** The `PosManager.cs` simplifies complex race ranking maths into a single "Position" value for the UI, while `AIController.cs` hides complex navigation math from the engine.
* **Polymorphic Logic:** Utilized **enums and switch-case structures** in `PowerUpManager.cs` to manage diverse combat behaviour through a unified activation interface.
* **Decoupled Systems:** Heavy use of specialised managers for power-ups, race state, camera views, and UI to ensure a high degree of modularity.


## Project Structure

* **`Assets/Scripts/`**: Core C# logic including `AIController`, `CarController`, `RaceManager`, and `PosManager`.
* **`Packages/`**: Essential manifest files defining project dependencies.
* **`ProjectSettings/`**: Global configurations, including custom tags (`AI`, `Car`, `BRAKE`) and Input System bindings.


## How to Run Locally

1. **Clone the repo:**

```bash
git clone https://github.com/mahraz-ahmed/adrenaline-rush.git

```

2. **Open in Unity:** Ensure you are using **Unity 6000.0.25f1**.
3. **Git LFS:** This project uses Git LFS for high-resolution textures. Ensure LFS is installed to view full art assets.
4. **Load Scene:** Open `Assets/Scenes/Main Menu.unity`.
5. **Press Play.**

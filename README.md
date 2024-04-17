## Directory Tree
```Clock Calamity  
├── Characters  
│   ├── Player  
│   │   ├── Pistol.tres  
│   │   ├── player.tscn  
│   │   ├── PlayerController.cs  
├── Components  
│   ├── CustomCursor  
│   │   ├── CustomMouseComponent.cs  
│   ├── Game  
│   │   ├── GameStats.cs  
│   │   ├── StatsComponent.cs  
│   ├── Pathfinding  
│   │   ├── AStarGrid2DComponent.cs  
│   │   ├── FollowWaypointsComponent.cs  
│   │   ├── GridResource.cs  
│   │   ├── WaypointsResource.cs  
│   ├── Weapons  
│   │   ├── WeaponComponent.cs  
│   │   ├── WeaponResource.cs  
├── Debug  
│   ├── Debug2D.cs  
├── Enemies  
│   ├── Enemy.cs  
│   ├── NPCResource.cs  
│   ├── solider_1.tscn  
│   ├── SpawnPoint.cs  
├── Game  
│   ├── GameStatsResource.tres  
├── Levels  
│   ├── Level01  
│   │   ├── EnemyLeft.tres  
│   │   ├── level_01.tscn  
│   │   ├── Occupied.tres  
│   │   ├── PathLeft.tres  
│   │   ├── PathsRight.tres  
│   ├── Hide.cs  
│   ├── LevelController.cs  
├── Projectiles  
│   ├── Bullet.cs  
│   ├── bullet.tscn  
│   ├── enemy_bullet.tscn  
├── Shaders  
│   ├── ui_ammo_empty.gdshader  
│   ├── ui_ammo_full.gdshader  
├── Sprites  
│   ├── Characters  
│   │   ├── Hitman 1  
│   │   │   ├── hitman1_gun.png  
│   │   │   ├── hitman1_hold.png  
│   │   │   ├── hitman1_machine.png  
│   │   │   ├── hitman1_reload.png  
│   │   │   ├── hitman1_silencer.png  
│   │   │   ├── hitman1_stand.png  
│   │   ├── Soldier 1  
│   │   │   ├── soldier1_gun.png  
│   │   │   ├── soldier1_hold.png  
│   │   │   ├── soldier1_machine.png  
│   │   │   ├── soldier1_reload.png  
│   │   │   ├── soldier1_silencer.png  
│   │   │   ├── soldier1_stand.png  
│   │   ├── Soldier 2  
│   │   │   ├── soldier2_gun.png  
│   │   │   ├── soldier2_hold.png  
│   │   │   ├── soldier2_machine.png  
│   │   │   ├── soldier2_reload.png  
│   │   │   ├── soldier2_silencer.png  
│   │   │   ├── soldier2_stand.png  
│   ├── Projectiles  
│   │   ├── spaceMissles_033.png  
│   ├── UI  
│   │   ├── crosshair161.png  
├── Tilesets  
│   ├── tilesheet_complete.png  
├── UI  
│   ├── UI_Ammo.cs  
│   ├── ui_ammo.tscn  
├── icon.svg  
```

## Scene Trees

### Level 1
```
Level01 📜  
├── TileMap  
├── Player 🎬📜  
├── PlayerHides  
│   ├── HideTree 📜  
│   │   ├── HideMarker  
│   │   ├── LeftMarker  
│   │   ├── RightMarker  
├── AStarGrid2DComponent 📜  
├── SpawnPoints  
│   ├── SpawnMarkerLeft 📜  
│   ├── SpawnMarkerRight 📜  
├── Enemies  
├── StatsComponent  
├── Debug  
```

### Player
```
Player 📜  
├── Anchor  
│   ├── AnimatedSprite2D  
│   ├── MuzzleMarker  
├── CustomMouseComponent 📜  
├── WeaponComponent 📜  
```

### Soldier 1
```
Soldier1 📜  
├── Anchor  
│   ├── AnimatedSprite2D  
│   ├── MuzzleMarker  
├── Area2D  
│   ├── CollisionShape2D  
├── FollowWaypointsComponent 📜  
├── StatsComponent 📜  
```

### Bullet / Enemy Bullet
```
Bullet 📜  
├── Sprite2D  
├── VisibleOnScreenNotifier2D  
├── Area2D  
│   ├── CollisionShape2D  
```

### UI Ammo
```
UI_Ammo 📜  
├── AmmoHBox  
```

### Backlog
1. Do I need time between shots for player?
2. path follow component
3. UI for ammo
4. npc resource to ai resource
5. enemy TLS handles movement - should be an ai movement component of some kind
6. SpawnComponent is really not a reusable component
7. maybe an interface for projectiles?
8. UI health, ammo should be one component
9. Need to add reload for enemies?

### Next Steps
6. Aim Component
7. Refactor Debug2D
8. Rename namespaces to 2D instead of components?
9. Need to avoid moving to an occupied space
10. Add sound effects
12. Switching WeaponComponent
13. Ammo in UI
14. Player take damage
15. Health in UI
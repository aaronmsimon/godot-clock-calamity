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
5. enemy TLS handles movement - should be an ai movement component of some kind
8. UI health, ammo should be one component
9. Need to add reload for enemies?
10. Rename namespaces to 2D instead of components?
11. Aim Component
12. Spawn Component
13. Does player need splat?
14. Clean up game over screen

### Next Steps
7. Refactor Debug2D
9. Add sound effects - hit
10. Switching WeaponComponent
12. Need game over screen with scores and option to try again
13. need title screen and have it take you to level 1 (for now)
14. try again is hard-coded
15. need to handle player death vs next level vs game over vs queue free vs dying while having killed last enemy
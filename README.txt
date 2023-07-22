--- QUIZ README ---

MADE BY: Jeremy Poulin (40112762)

The following behaviours have been defined for the quiz:

Movements:
- Player movement (left - right - jump)
- (No movements were implemented for the AI)

Weapons:
- The three weapons have been implemented with their respective trajectory
- Switching of weapons is done by pressing the F key
- The trajectory of the projectile is shown on the screen with the use of dots\
- The projectiles are moved with the use of SELF-IMPLEMENTED physics 
  (calculations based on mouse placement and type of weapon)

AI:
- AI agents target the closest player, aim for them, and shoot with a random weapon
  (No other implementation was done due to time constraints)

Rules:
- Any agent hit by a projectile is killed (friendly fire or not)
- Any agent falling off the map is killed
- When a full team is killed, the game ends
- When the game timer reaches zero, the game ends

Additional Info:
- Main Menu Scene
- Pause / Resume Functionality 
- Play Again Functionality
- Auto End Game in the pause menu
- Auto End Game when the timer runs out
- The camera follows the active player and projectile when shot


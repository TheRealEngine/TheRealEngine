namespace TheRealEngine.UniversalRendering.Input;

public class UpdateToTickJustPressedHandler(Func<KeyboardButton, bool> isJustPressedThisUpdateFunc) {
    private readonly HashSet<KeyboardButton> _newJustPressed = new(16);  // 16 should be enough for most ticks
    private readonly HashSet<KeyboardButton> _currentJustPressed = new(16);  // 16 should be enough for most ticks
    
    public void Update() {
        foreach (KeyboardButton button in Enum.GetValues<KeyboardButton>()) {
            if (isJustPressedThisUpdateFunc(button)) {
                _newJustPressed.Add(button);
            }
        }
    }

    public void Tick() {
        // Clear current just pressed and move new to current
        _currentJustPressed.Clear();
        foreach (KeyboardButton button in _newJustPressed) {
            _currentJustPressed.Add(button);
        }
        _newJustPressed.Clear();
    }
    
    public bool IsButtonJustPressedThisTick(KeyboardButton button) {
        return _currentJustPressed.Contains(button);
    }
}

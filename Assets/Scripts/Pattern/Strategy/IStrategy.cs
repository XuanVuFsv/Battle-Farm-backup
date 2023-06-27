// The Command interface declares a method for executing a command.
public interface IWeaponStragety
{
    public void HandleLeftMouseClick();
    public void HandleRightMouseClick();
    public void SetInputData(object _inputData);
}

public interface IPrimaryWeaponStragety : IWeaponStragety
{
    ShootingInputData GetShootingInputData();
}

public interface IHandGunWeaponStragety : IWeaponStragety
{
    
}
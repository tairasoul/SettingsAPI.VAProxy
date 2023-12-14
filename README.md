# SettingsAPI

A mod settings API for VA Proxy.

In progress.

Feel free to contribute.

## How to use

This mod has multiple methods, accessible through Plugin.API.

Up first, you have RegisterMod().

RegisterMod registers your mod to be added to the settings menu.

You can provide an initial create callback (to setup the mini-page assigned to the mod), or you can leave it out.

Options are the buttons displayed within the mini-page assigend to the mod.

Example:

```cs
Option option = new Option() {
    Create = (GameObject page) => {
        GameObject button = ComponentUtils.CreateButton("test!!", "test.id.mod");
        button.GetComponent<Button().onClick.AddListener(() => Logger.LogInfo("Button pressed!"));
        button.SetParent(page, false);
    }
};
Option[] options = Array.Empty<Option>();
options = options.Append(option);
SettingsAPI.Plugin.API.RegisterMod("com.example.mod", "example-mod", options);
```

For basic registration, that's all.

For utils exposed by it, we've got extensions and component utils.

Component utils exposes 4 functions, and will expose more as more UI element types are added to the game.

Currently, there's GetFont(FontName), CreateToggle(displayText, ObjectName), CreateButton(displayText, ObjectName) and CreateSlider(displayText, ObjectName)

GetFont simply returns a font by name.

Create____ returns a GameObject, not parented to anything and with just the basics setup.

You'll have to parent it and setup different events.

CreateToggle will automatically enable/disable the checkmark depending on status of the toggle.

If a part of the setup for the functions is not to your liking, you can modify it after creation.

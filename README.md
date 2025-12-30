A complete Unity Editor theming toolkit with visual IMGUI inspectors and automatic USS generation.

# Installation
1. Download the latest .unitypackage from [Releases](https://github.com/Marmik-Emp37/Emp37.Editor-Theme/releases)
2. Import it into your project
3. Done!


# Using Example Themes
To get started quickly, use one of the included example themes as building a theme from scratch typically takes more time and requires deeper familiarity with Unity’s USS selector system. Three production-ready themes are provided in this repository and can be used out of the box or edited to your preference.

| **Quick Setup** |
1. Locate the themes in your project _`Assets/Emp37.Editor-Theme/Example Themes/`_
2. Select a theme asset in the project window
3. Navigate to the Inspector and make sure `Recompile On Apply` is set to true
4. Click **Apply Theme** button

## Available Themes
### Greyscale (Dark)
A clean, minimalist monochrome design that's easy on the eyes during long coding sessions.

<img width="1920" height="960" alt="{AF69BE20-D2C3-4D18-A1C2-2FFCE5464ABE}" src="https://github.com/user-attachments/assets/3e5ae99b-46da-4b3f-bd61-d2b71342342d"/>

### Penelope (Light)
A balanced, professional theme with a carefully crafted color scheme.

<img width="1920" height="960" alt="{91FD4779-95C8-4A45-BF3F-0A08C3FBB773}" src="https://github.com/user-attachments/assets/9ed26167-c6cc-4762-8be2-b06cdebe2f1e"/>

### Scyther (Light)
Sharp, modern styling with a bold visual language.


# Advanced
## Creating a New Theme
1. **Create a Theme asset**  
   Right-click in the Project window: `Create → Editor-Theme → (New Light Theme / New Dark Theme)`

2. **Add Style Rules**  
   Each rule consists of:
   - **Selectors**: class names like `button`, `label`, `toolbar`
   - **Pseudo Classes**: states like `:hover`, `:active`, `:focus`, etc.
   - **Property Mask**: choose which CSS properties to define from **property values** as:
      - Background image & color
      - Border colors (unified or per-side)
      - Border radius & width (px or %)
      - Text color

3. **Organize with Style Groups**
   - Group related rules together
   - Enable/disable entire groups
   - Add descriptive titles
   - Collapse groups to reduce clutter

4. **Apply your theme to automatically -**
   - Generate a `.uss` file under `Assets/Editor/StyleSheets/Extensions/`
   - Switch Unity to the selected skin mode
   - Update the selection color
   - Optionally trigger a script recompilation

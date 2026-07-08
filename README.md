Editor-Theme is a high-level editor tool that simplifies the customization of IMGUI elements, allowing you to transform the look and feel of the Unity Editor with zero coding required. Design, organize, and apply custom themes to create a visually refreshing workspace and enhance your daily development experience.

## Installation
1. Download the latest .unitypackage from [Releases](https://github.com/Marmik-Emp37/Emp37.Editor-Theme/releases)
2. Import it into your project
3. Done!

## Using Example Themes
Building a theme from scratch requires familiarity with Unity’s USS selector system. To get started immediately, I recommend using one of the three production-ready templates included in this repository. They can be used out-of-the-box or tweaked to your preference.

### Quick Setup
1. Locate the example themes in your project under `Assets/Emp37.Editor-Theme/Example Themes/`
2. Select a theme asset in the Project window.
3. In the Inspector, ensure **Recompile On Apply** is enabled.
4. Click the **Apply Theme** button.

### Available Themes
1. **Greyscale (Dark)** A clean, minimalist monochrome design that is easy on the eyes during long coding sessions.  
<img width="1920" height="960" alt="Greyscale Theme" src="https://github.com/user-attachments/assets/3e5ae99b-46da-4b3f-bd61-d2b71342342d"/>

2. **Penelope (Light)** A balanced, professional theme with a carefully crafted, warm color scheme.  
<img width="1920" height="960" alt="Penelope Theme" src="https://github.com/user-attachments/assets/9ed26167-c6cc-4762-8be2-b06cdebe2f1e"/>

3. **Scyther (Light)** Sharp, modern styling featuring a bold visual language.  
<img width="1920" height="960" alt="Scyther Theme" src="https://github.com/user-attachments/assets/e8eba454-8d4b-478c-9e0c-76094cd253c9" />

## Advanced: Creating a Custom Theme
### 1. Create a Theme Asset
Right-click in the Project window and navigate to:  
`Create → Editor-Theme → (New Light Theme / New Dark Theme)`

### 2. Add Style Rules
Construct your theme by stacking rules. Each rule consists of:
* **Selectors:** Target specific elements (e.g., `button`, `label`, `toolbar`).
* **Pseudo-Classes:** Define interaction states (e.g., `:hover`, `:active`, `:focus`).
* **Property Mask:** Expose only the properties you want to change, keeping the inspector clean:
  * Background image & color
  * Border colors (unified or per-side)
  * Border radius & width (`px` or `%`)
  * Text color

### 3. Organize with Style Groups
Keep your theme asset maintainable as it grows:
* Group related UI elements together.
* Add descriptive titles for quick navigation.
* Collapse or enable/disable entire groups with a single click to isolate changes.

### 4. Apply & Generate
When you click **Apply Theme**, the tool automatically handles the heavy lifting:
* Generates a compiled `.uss` file under `Assets/Editor/StyleSheets/Extensions/`.
* Switches the Unity Editor to the appropriate Light/Dark skin mode.
* Updates the global selection color.
* Triggers a safe script recompilation (if enabled) to apply changes flawlessly.

## Built-in Utilities: GUI Style Explorer
Finding the exact internal names of Unity's UI elements (like `MeTransitionHead` or `RL Header`) to use as selectors can be a frustrating with IMGUI Debugger. To solve this, Editor-Theme includes a built-in discovery tool.

**Open it via:** `Tools → Emp37 → ET.GUI Style Explorer`

<img width="453" height="558" alt="GUIStyleExplorer Example" src="https://github.com/user-attachments/assets/317c85e7-4aa6-45c7-8e74-b1ef26931d3c"/>

* **Search & Discover:** Instantly filter through hundreds of hidden internal GUIStyles available in your current Unity skin.
* **Live Preview:** View exactly how a style renders. Toggle `Focus`, `Hover`, and `Active` states, and test it with custom sample text.
* **One-Click Copy:** Double-click any style in the list to automatically copy its exact, formatted internal name to your clipboard-ready to be pasted straight into your Theme's **Selectors** list.

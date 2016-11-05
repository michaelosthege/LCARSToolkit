# LCARSToolkit
User controls for building UWP apps with a LCARS-like GUI.

## Purpose
Building Windows Universal apps for fancy IoT and home automation.

## Related Work
For many years people have built things that resemble the LCARS interface we all know and love. The following example ([link to GitHub repo](https://github.com/DieterKoblenz/LCARS)) is on YouTube since 2010, long before IoT and home automation was a thing.

(image links to YouTube video)

[![YouTube link to LCARS home automation](http://img.youtube.com/vi/2vOvDFxn76g/0.jpg)](http://www.youtube.com/watch?v=2vOvDFxn76g "Star Trek-like home automation")

## Usage
You can either fork this repo directly, or [grab the package from NuGet](https://www.nuget.org/packages/LCARSToolkit.Controls/).

Add a reference to the LCARSToolkit namespace:

```XAML
xmlns:lcars="using:LCARSToolkit.Controls"
```

At the moment there are four controls:


![Image](./screenshot.png?raw=true)

* **Stump**: used to decorate ends
* **Elbo**: traditional corners / layouting
* **Button**: inherits the standard Button and adds illumination/flashing and Stumps
* **LabeledButton**: see screenshot. Was a nightmare to build. Supports LeftToRight and RightToLeft flow directions and Stumps on either end.
* **Rectangle** : well guess what. (for illumination of rectangles in the layout)

All controls have an Illumination property and can flash in synchrony.


### Font & Colors
The controls are looking for a StaticResource with x:Key="LCARS", so we have to define it in the App/Page resources.

The 'real' LCARS font is called *Swiss 911 Ultra Compressed* and copyrighted, so you have to provide it yourself or use an alternative.

```XAML
<Application.Resources>
    <!--App-wide setting of the desired font family-->
    <FontFamily x:Key="LCARS">Haettenschweiler</FontFamily>
    <!--the 'real' LCARS font is called Swiss 911 Ultra Compressed. If you do have a copy:-->
    <!--<FontFamily x:Key="LCARS">Resources/Swiss 911 Ultra Compressed.ttf#LCARS</FontFamily>-->

    <SolidColorBrush x:Key="Static" Color="#F1DF6F" />
    <SolidColorBrush x:Key="Offline" Color="#FF0000" />
    <SolidColorBrush x:Key="Unavailable" Color="#3399FF" />
    <SolidColorBrush x:Key="Primary" Color="#99CCFF" />
    <SolidColorBrush x:Key="Yellow" Color="#FFFF33" />
    <SolidColorBrush x:Key="Orange" Color="#FF9900" />
        
    <!--MediaElements need to be part of the Visual Tree to work. That's why we can't put them in App.Resources-->
</Application.Resources>
```

### Sounds
This repository does not contain sound samples to avoid legal complications. You can [grab the original sounds from lcarscom.net](http://www.lcarscom.net/sounds.htm). Should this link ever die, don't hesitate to contact me for a backup ;)

If you know a source of nice-sounding click an beep sounds under a permissive license, please contribute a link!

Each control has a SoundElement property that you can bind to a MediaElement to provide a custom soundeffect:

```XAML
<Page.Resources>
    <!--File Properties: Content, Copy if Newer-->
    <!--MediaElements need to be part of the Visual Tree to work. That's why we can't put them in App.Resources-->
    <MediaElement x:Key="Beep01" Source="Resources/Sounds/Beep01.wav" AutoPlay="False" />
    <MediaElement x:Key="Click01" Source="Resources/Sounds/Click01.wav" AutoPlay="False" />
</Page.Resources>

<lcars:Button Content="Custom Beep" SoundElement="{StaticResource BeepSound}" Background="{StaticResource Orange}" />
```

## Contributing
Please do contribute! Ideally you'd fork this repo, make changes in your branch and submit a pull request.

Here are some thigs on the ToDo list:
* TextBox
* PasswordBox
* ComboBox
* RadioButton
* CheckBox

Please open issues for other things you find relevant.

## Legal
This repository is a non-commercial implementation of user controls to reproduce the look and feel of LCARS-GUIs. Due to its non-commercial nature, it is considered to be fair-use as per [17 U.S. Code § 107](https://www.law.cornell.edu/uscode/text/17/107).

### Can I make apps for personal use?
You are encouraged to build applications for personal use, for example IoT interfaces as seen in the video linked above.

### Can I publish apps with it?
You probably shouldn't. CBS holds a copyright claim on the LCARS GUI and has taken down apps looking like LCARS in the past ([link](http://www.pocketables.com/2011/04/cbs-claiming-copyright-on-star-trek-computer-interface-used-in-ipad-app.html)).



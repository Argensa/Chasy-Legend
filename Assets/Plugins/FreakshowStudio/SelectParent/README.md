
Select Parent
=============

    © 2011-2015 Freakshow Studio AS
    All rights reserved


Introduction
------------

This is a small editor extension for Unity for quickly and easily selecting the parent of an object.

The parent object, or the root object can easily be selected with a hotkey.

Also included is a component you can add to a gameobject that will automatically select its parent when you try to select it.

This is useful when you for example have a hierarchy with a scene selectable object as a child, when you don't want to move this object in local space but instead always move its parent.


Usage
-----

In the editor, you can always press ```Alt+P``` or ```Alt+Shift+P``` to select either the parent, or the root gameobject of your current selection.

You can also add the component found under ```Component -> Select Parent -> Always Select My Parent```, or just add the ```AlwaysSelectMyParent.cs``` script to an object. Since you sometimes will want to select these objects anyway, toggle the functionality from the ```Edit -> Always Select Parent``` menu. You can also toggle it on and off using the hotkey ```Alt+Shift+Control+P```.


Hotkeys
-------

| Key                       | Function                    |
|---------------------------|-----------------------------|
| ```Alt+P```               | Select Parent               |
| ```Alt+Shift+P```         | Select Root                 |
| ```Alt+Shift+Control+P``` | Toggle Always Select Parent |

On Mac, substitute ```Alt``` with ```Option``` and ```Control``` with ```Command```.


Support
-------

Should you require assistance, please contact <support@freakshowstudio.com>

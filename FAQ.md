# Frequently Asked Questions

## ''/* Array type not detected. */''

This can be fixed by defining an array's type, these definitions can be configured under the Options tab, 
in the options tab look to right, there should be a section that reads “Array Type Definitions”, 
in this section you can define array types to assist the decompiler with.

For example the property “ColorScale” is an array property with an inner type of “StructProperty”, 
so we can tell UE Explorer this by adding a new definition with the input of “Package.Class.ColorScale:StructProperty”. 
(Package.Class doesn’t really matter, it’s supposed to be the class and package where this property is defined.)

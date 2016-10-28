# ZippingEasy
Tool to create zip files from folders in the deepest location of a folder system

Imagine you have a specific folder structure:

Root
--- Sub 1
    --- Sub Sub 1.1
    --- Sub Sub 1.2
      --- Sub Sub Sub 1.1.1
    --- Sub Sub 1.3
--- Sub 2
    --- Sub Sub 2.1
        
        ...

And now you want to create zip archives from the deepest subfolders in this structure - let's say to create a backup of everything.
So, instead of going down to the deepest location and create the archives manually, you can tell this program what your Root folder 
is and it will crawl down to thw core and create zip archives on a specific destination directory.

This Tool was made with C# and WPF
Used 3rd Party Libs are Caliburn.Micro, DevExpress Mvvm, WPF Toolkit, log4net, icon8, and SevenZipSharp. More might come when the Project is getting further

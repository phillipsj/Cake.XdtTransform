# Cake.XdtTransform

This was adapted from an [open issue](https://github.com/cake-build/cake/issues/321) on Github that shows 
how to perform an XDT config transform. The issue is still open, but I didn't there would need to be an 
extra dependency pulled in unless you really needed it and that is why it is still open.

```csharp
#addin Cake.XdtTransform

var target = Argument("target", "Default");

Task("TransformConfig")
  .Does(() =>
{
  var sourceFile      = File("web.config");
  var transformFile   = File("web.release.config");
  var targetFile      = File("web.target.config");
  XdtTransformConfig(sourceFile, transformFile, targetFile);
});

RunTarget(target);
```
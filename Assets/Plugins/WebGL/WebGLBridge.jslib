mergeInto(LibraryManager.library, {
  MyJSFunction: function (param1, param2) {
    console.log("JS function called from Unity!");
    console.log("param1:", param1);
    console.log("param2:", UTF8ToString(param2));
  }
});

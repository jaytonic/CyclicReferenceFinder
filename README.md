# CyclicReferenceFinder
This is a small tool designed to help user to find the "project reference chain" that prevent the user to add a new project in VisualStudio.

I've made a simple GUI to easily select your own Visual Studio Solution file(*.sln), it will then allow you to select the project in which you are trying to add a reference, and the project which you're trying to reference.

This tool have been made because I was unable to find the reason I could not add a project reference(due to cyclic dependency).
I don't expect to be very robust for now, but if you have issue, don't hesitate to raise an issue.

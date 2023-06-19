# The Cy Compiler
Making C look a bit different. It works like SASS, where you have a file with cooler syntax and fancy features that you can compile to a functional .css file.
The difference is that Cy has almost the same syntax as C and has no fancy features.
## Getting started
- Install the Cy Compiler through the [releases](https://github.com/ariaoblige/cy-cmp/releases) page.
- Put it into a safe folder.
- Add that folder to the environment variables.
- Type `cy` in the terminal to create a new project or compile it to C.
**Note: Cy expects the compile functionality to be executed at the root directory of the project, not the src directory.**
---
## Syntax differences
All the four default C data types are introduced by @ and are only two characters long. When formatting strings, **use an $ instead of %**.

| C                 | Cy                |
| ----------------- | ----------------- |
| `int`             | `@in`             |
| `double`          | `@db`             |
| `float`           | `@fl`             |
| `char`            | `@ch`             |
| `printf("%d", 6)` | `printf("$d", 6)` |

Other than the listed above, Cy syntax is the same as C.

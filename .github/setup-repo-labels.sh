#!/bin/bash

# Pull request type labels

gh label create "chore" -d "A change that doesn't affect production code" -c "#c5def5" -f
gh label create "bug" -d "Something isn't working" -c "#d73a4a" -f
gh label create "enhancement" -d "Improvement to an existing feature" -c "#a2eeef" -f
gh label create "feature" -d "New feature" -c "#a2eeef" -f

# Versioning labels

gh label create "patch" -d "Increment the patch version number (0.0.x)" -c "#bfd4f2" -f
gh label create "minor" -d "Increment the minor version number (0.x.0)" -c "#fbca04" -f
gh label create "major" -d "Increment the major version number (x.0.0)" -c "#0e8a16" -f

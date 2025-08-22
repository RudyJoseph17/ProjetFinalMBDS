import os
import sys
sys.path.insert(0, os.path.abspath('..'))

project = 'Projet Final MBDS'
author = 'Rudy Joseph'
release = '1.0.0'

extensions = [
    'myst_parser',        # permet de lire les fichiers .md
    'sphinx.ext.autodoc',
    'sphinx.ext.viewcode',
]

templates_path = ['_templates']
exclude_patterns = []

html_theme = 'sphinx_rtd_theme'
html_static_path = ['_static']

# Déclaration des suffixes de source
source_suffix = {
    '.rst': 'restructuredtext',
    '.md': 'markdown',
}
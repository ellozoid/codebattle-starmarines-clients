from distutils.core import setup

setup(name='starmarinesclient',
      version='1.0',
      description='Hardcoded Star Marines game client',
      author='',
      author_email='',
      packages=['starmarinesclient'],
      install_requires=['websocket-client', 'click'],
      entry_points={
          'console_scripts': [ 'starmarinesclient=starmarinesclient.CodeBattlePython:main']
      }
)

# pokedex

## Build Project

``` 
git clone https://github.com/jernejg/pokedex.git
cd pokedex
docker build -t jernej/pokedex:latest -f ./src/Pokedex.Api/Dockerfile .
```

## Run Project
``` docker run -p 5000:80 -e ASPNETCORE_ENVIRONMENT="Development" --name pokedex -d jernej/pokedex ```

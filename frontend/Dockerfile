FROM node:16

# Ustawienie katalogu roboczego w kontenerze
WORKDIR /app

COPY package*.json ./

# Instalacja zależności
RUN npm install

COPY . .

RUN npm install -g http-server

# Eksponowanie portu AngularJS
EXPOSE 4200

CMD ["http-server", ".", "-p", "4200"]
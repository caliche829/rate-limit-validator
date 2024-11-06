# ----------------------------
# build from source
# ----------------------------
FROM node AS build

WORKDIR /app

COPY package*.json .
RUN npm install npm@9.7.1 -g
RUN npm install

COPY . .
RUN npm run build  -- --c production

# ----------------------------
# run with nginx
# ----------------------------
FROM nginx
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d
RUN rm -rf /usr/share/nginx/html/* 
COPY --from=build /app/dist/request-report/browser /usr/share/nginx/html
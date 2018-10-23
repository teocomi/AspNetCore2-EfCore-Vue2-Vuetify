<template>
    <v-dialog :value="value" width="800px" @input="$emit('input')">
        <v-card>
            <v-card-title primary-title>
                <h1>Share via link</h1>
            </v-card-title>
            <v-card-text>
                <v-layout row>
                    <v-text-field :value="fullurl()"
                                  class="share-input"
                                  label="URL"
                                  box
                                  readonly></v-text-field>
                    <v-btn id="calc-btn" color="black" flat fab @click.native.stop="copyToClipboard(fullurl())">
                        <v-icon>content_copy</v-icon>
                    </v-btn>
                </v-layout>
              
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="grey darken-1" flat @click.native="$emit('input')">Close</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    export default {
        props: ['url','value'],
        data() {
            return {
            }
        },


        methods: {

            fullurl: function () {
                return window.location.origin +'/'+ this.url;
            } ,
            copyToClipboard(str) {
                const el = document.createElement('textarea');  // Create a <textarea> element
                el.value = str;                                 // Set its value to the string that you want copied
                el.setAttribute('readonly', '');                // Make it readonly to be tamper-proof
                el.style.position = 'absolute';
                el.style.left = '-9999px';                      // Move outside the screen to make it invisible
                document.body.appendChild(el);                  // Append the <textarea> element to the HTML document
                const selected =
                    document.getSelection().rangeCount > 0        // Check if there is any content selected previously
                        ? document.getSelection().getRangeAt(0)     // Store selection if found
                        : false;                                    // Mark as false to know no selection existed before
                el.select();                                    // Select the <textarea> content
                document.execCommand('copy');                   // Copy - only works as a result of a user action (e.g. click events)
                document.body.removeChild(el);                  // Remove the <textarea> element
                if (selected) {                                 // If a selection existed before copying
                    document.getSelection().removeAllRanges();    // Unselect everything on the HTML document
                    document.getSelection().addRange(selected);   // Restore the original selection
                }
            },
        }
    }
</script>

<style>
    .share-input i--click::after {
        animation: anim-effect-boris 0.3s forwards;
        background-color:red
    }

    @keyframes anim-effect-boris {
        0% {
            transform: scale3d(0.3, 0.3, 1);
        }

        25%, 50% {
            opacity: 1;
        }

        100% {
            opacity: 0;
            transform: scale3d(1.2, 1.2, 1);
        }
    }
</style>

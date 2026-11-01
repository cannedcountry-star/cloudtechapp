package com.cannedcountry.cloudtech

import android.content.ActivityNotFoundException
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.widget.Toast
import androidx.browser.customtabs.CustomTabColorSchemeParams
import androidx.browser.customtabs.CustomTabsIntent
import androidx.core.content.ContextCompat

object UrlOpener {

    /**
     * 指定されたURLを Chrome Custom Tabs（またはデフォルトブラウザ）で開く
     */
    fun openUrl(context: Context, url: String) {
        val uri = Uri.parse(url)

        val primaryColor = ContextCompat.getColor(context, R.color.secondary)
        val defaultColorParams = CustomTabColorSchemeParams.Builder()
            .setToolbarColor(primaryColor)
            .build()

        val customTabsIntent = CustomTabsIntent.Builder()
            .setDefaultColorSchemeParams(defaultColorParams)
            .setShowTitle(true)
            .setUrlBarHidingEnabled(true)
            .build()

        try {
            // Chromeが利用可能な場合はChromeパッケージを指定
            customTabsIntent.intent.setPackage("com.android.chrome")
            if (context !is android.app.Activity) {
                customTabsIntent.intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            }
            customTabsIntent.launchUrl(context, uri)
        } catch (e: Exception) {
            // Chromeがなければ汎用のCustom Tabs / ブラウザで開く
            try {
                val fallbackIntent = CustomTabsIntent.Builder()
                    .setDefaultColorSchemeParams(defaultColorParams)
                    .setShowTitle(true)
                    .build()
                if (context !is android.app.Activity) {
                    fallbackIntent.intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
                }
                fallbackIntent.launchUrl(context, uri)
            } catch (fallbackEx: Exception) {
                // 最終フォールバック：標準のブラウザIntent
                try {
                    val viewIntent = Intent(Intent.ACTION_VIEW, uri).apply {
                        if (context !is android.app.Activity) {
                            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
                        }
                    }
                    context.startActivity(viewIntent)
                } catch (finalEx: ActivityNotFoundException) {
                    Toast.makeText(context, "ブラウザを開けませんでした", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}

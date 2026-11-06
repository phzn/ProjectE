using System;

namespace DA_Assets.FCU
{
	// Token: 0x02000023 RID: 35
	public enum FcuLocKey
	{
		// Token: 0x040000A8 RID: 168
		log_added_total,
		// Token: 0x040000A9 RID: 169
		log_api_waiting,
		// Token: 0x040000AA RID: 170
		log_auth_complete,
		// Token: 0x040000AB RID: 171
		log_cant_auth,
		// Token: 0x040000AC RID: 172
		log_cant_draw_object,
		// Token: 0x040000AD RID: 173
		log_cant_find_package,
		// Token: 0x040000AE RID: 174
		log_cant_get_images,
		// Token: 0x040000AF RID: 175
		log_cant_get_part_of_frames,
		// Token: 0x040000B0 RID: 176
		log_cant_get_image_links,
		// Token: 0x040000B1 RID: 177
		log_cant_execute_because_no_backup,
		// Token: 0x040000B2 RID: 178
		log_component_not_selected_in_hierarchy,
		// Token: 0x040000B3 RID: 179
		log_current_canvas_metas_destroy,
		// Token: 0x040000B4 RID: 180
		log_dev_function_enabled,
		// Token: 0x040000B5 RID: 181
		log_downloading_fonts,
		// Token: 0x040000B6 RID: 182
		log_downloading_images,
		// Token: 0x040000B7 RID: 183
		log_drawn_count,
		// Token: 0x040000B8 RID: 184
		log_enable_http_project_settings,
		// Token: 0x040000B9 RID: 185
		log_fcu_assigned,
		// Token: 0x040000BA RID: 186
		log_generating_sprites,
		// Token: 0x040000BB RID: 187
		log_generating_tmp_fonts,
		// Token: 0x040000BC RID: 188
		log_getting_frames,
		// Token: 0x040000BD RID: 189
		log_getting_links,
		// Token: 0x040000BE RID: 190
		log_import_complete,
		// Token: 0x040000BF RID: 191
		log_import_task_canceled,
		// Token: 0x040000C0 RID: 192
		log_incorrect_selection,
		// Token: 0x040000C1 RID: 193
		log_instantiate_game_objects,
		// Token: 0x040000C2 RID: 194
		log_links_added,
		// Token: 0x040000C3 RID: 195
		log_local_prefabs_found,
		// Token: 0x040000C4 RID: 196
		log_malformed_url,
		// Token: 0x040000C5 RID: 197
		log_mark_as_sprite,
		// Token: 0x040000C6 RID: 198
		log_need_auth,
		// Token: 0x040000C7 RID: 199
		log_no_google_fonts_api_key,
		// Token: 0x040000C8 RID: 200
		log_no_sync_helper,
		// Token: 0x040000C9 RID: 201
		log_nothing_to_import,
		// Token: 0x040000CA RID: 202
		log_open_auth_page,
		// Token: 0x040000CB RID: 203
		log_prefabs_created,
		// Token: 0x040000CC RID: 204
		log_project_downloaded,
		// Token: 0x040000CD RID: 205
		log_project_empty,
		// Token: 0x040000CE RID: 206
		log_project_not_found,
		// Token: 0x040000CF RID: 207
		log_search_local_prefabs,
		// Token: 0x040000D0 RID: 208
		log_set_anchors,
		// Token: 0x040000D1 RID: 209
		log_ssl_error,
		// Token: 0x040000D2 RID: 210
		log_start_adding_to_fonts_list,
		// Token: 0x040000D3 RID: 211
		log_start_creating_prefabs,
		// Token: 0x040000D4 RID: 212
		log_start_download_images,
		// Token: 0x040000D5 RID: 213
		log_start_setting_transform,
		// Token: 0x040000D6 RID: 214
		log_tagging,
		// Token: 0x040000D7 RID: 215
		log_unknown_aligment,
		// Token: 0x040000D8 RID: 216
		log_unknown_error,
		// Token: 0x040000D9 RID: 217
		log_not_authorized,
		// Token: 0x040000DA RID: 218
		log_feature_not_available_with,
		// Token: 0x040000DB RID: 219
		log_incorrent_project_url,
		// Token: 0x040000DC RID: 220
		log_name_linking_not_recommended,
		// Token: 0x040000DD RID: 221
		log_svg_scale_1,
		// Token: 0x040000DE RID: 222
		log_import_failed_incompatible,
		// Token: 0x040000DF RID: 223
		log_import_failed_enable_required,
		// Token: 0x040000E0 RID: 224
		log_import_failed_unsupported,
		// Token: 0x040000E1 RID: 225
		log_tagging_by_parts,
		// Token: 0x040000E2 RID: 226
		log_asset_not_imported,
		// Token: 0x040000E3 RID: 227
		loading_google_fonts,
		// Token: 0x040000E4 RID: 228
		cant_download_fonts,
		// Token: 0x040000E5 RID: 229
		cant_generate_fonts,
		// Token: 0x040000E6 RID: 230
		cant_load_sprites,
		// Token: 0x040000E7 RID: 231
		cant_download_sprite,
		// Token: 0x040000E8 RID: 232
		label_advanced_mode,
		// Token: 0x040000E9 RID: 233
		tooltip_advanced_mode,
		// Token: 0x040000EA RID: 234
		label_add_tmp_fonts_from_folder,
		// Token: 0x040000EB RID: 235
		tooltip_add_fonts_from_folder,
		// Token: 0x040000EC RID: 236
		label_add_ttf_fonts_from_folder,
		// Token: 0x040000ED RID: 237
		tooltip_add_ttf_fonts_from_folder,
		// Token: 0x040000EE RID: 238
		label_all,
		// Token: 0x040000EF RID: 239
		tooltip_all,
		// Token: 0x040000F0 RID: 240
		label_apply_and_continue,
		// Token: 0x040000F1 RID: 241
		tooltip_apply_and_continue,
		// Token: 0x040000F2 RID: 242
		label_asset,
		// Token: 0x040000F3 RID: 243
		tooltip_asset,
		// Token: 0x040000F4 RID: 244
		label_asset_creator_settings,
		// Token: 0x040000F5 RID: 245
		tooltip_asset_creator_settings,
		// Token: 0x040000F6 RID: 246
		label_asset_dependencies,
		// Token: 0x040000F7 RID: 247
		tooltip_asset_dependencies,
		// Token: 0x040000F8 RID: 248
		label_atlas_padding,
		// Token: 0x040000F9 RID: 249
		tooltip_atlas_padding,
		// Token: 0x040000FA RID: 250
		label_atlas_population_mode,
		// Token: 0x040000FB RID: 251
		tooltip_atlas_population_mode,
		// Token: 0x040000FC RID: 252
		label_atlas_resolution,
		// Token: 0x040000FD RID: 253
		tooltip_atlas_resolution,
		// Token: 0x040000FE RID: 254
		label_auto_disable_compress_assets_on_import,
		// Token: 0x040000FF RID: 255
		tooltip_auto_disable_compress_assets_on_import,
		// Token: 0x04000100 RID: 256
		label_auto_size,
		// Token: 0x04000101 RID: 257
		tooltip_auto_size,
		// Token: 0x04000102 RID: 258
		label_best_fit,
		// Token: 0x04000103 RID: 259
		tooltip_best_fit,
		// Token: 0x04000104 RID: 260
		label_beta_version,
		// Token: 0x04000105 RID: 261
		tooltip_beta_version,
		// Token: 0x04000106 RID: 262
		label_buggy_version,
		// Token: 0x04000107 RID: 263
		tooltip_buggy_version,
		// Token: 0x04000108 RID: 264
		label_button_settings,
		// Token: 0x04000109 RID: 265
		tooltip_button_settings,
		// Token: 0x0400010A RID: 266
		label_button_type,
		// Token: 0x0400010B RID: 267
		tooltip_button_type,
		// Token: 0x0400010C RID: 268
		label_change,
		// Token: 0x0400010D RID: 269
		tooltip_change,
		// Token: 0x0400010E RID: 270
		label_changed_in_figma,
		// Token: 0x0400010F RID: 271
		tooltip_changed_in_figma,
		// Token: 0x04000110 RID: 272
		label_changed_in_unity,
		// Token: 0x04000111 RID: 273
		tooltip_changed_in_unity,
		// Token: 0x04000112 RID: 274
		label_comparer_desc,
		// Token: 0x04000113 RID: 275
		tooltip_comparer_desc,
		// Token: 0x04000114 RID: 276
		label_components_settings,
		// Token: 0x04000115 RID: 277
		tooltip_import_components,
		// Token: 0x04000116 RID: 278
		label_compression_quality,
		// Token: 0x04000117 RID: 279
		tooltip_compression_quality,
		// Token: 0x04000118 RID: 280
		label_copy_new_data,
		// Token: 0x04000119 RID: 281
		tooltip_copy_new_data,
		// Token: 0x0400011A RID: 282
		label_copy_old_data,
		// Token: 0x0400011B RID: 283
		tooltip_copy_old_data,
		// Token: 0x0400011C RID: 284
		label_copy_to_clipboard,
		// Token: 0x0400011D RID: 285
		tooltip_copy_to_clipboard,
		// Token: 0x0400011E RID: 286
		label_crunched_compression,
		// Token: 0x0400011F RID: 287
		tooltip_crunched_compression,
		// Token: 0x04000120 RID: 288
		label_csv_separator,
		// Token: 0x04000121 RID: 289
		tooltip_csv_separator,
		// Token: 0x04000122 RID: 290
		label_custom_pivot,
		// Token: 0x04000123 RID: 291
		tooltip_custom_pivot,
		// Token: 0x04000124 RID: 292
		label_dabutton_settings,
		// Token: 0x04000125 RID: 293
		tooltip_dabutton_settings,
		// Token: 0x04000126 RID: 294
		label_debug_mode,
		// Token: 0x04000127 RID: 295
		tooltip_debug_mode,
		// Token: 0x04000128 RID: 296
		label_debug_tools,
		// Token: 0x04000129 RID: 297
		label_debug,
		// Token: 0x0400012A RID: 298
		tooltip_debug_tools,
		// Token: 0x0400012B RID: 299
		label_dependencies,
		// Token: 0x0400012C RID: 300
		tooltip_dependencies,
		// Token: 0x0400012D RID: 301
		label_disable_compress_assets_on_import,
		// Token: 0x0400012E RID: 302
		tooltip_disable_compress_assets_on_import,
		// Token: 0x0400012F RID: 303
		label_disabled_color,
		// Token: 0x04000130 RID: 304
		tooltip_disabled_color,
		// Token: 0x04000131 RID: 305
		label_dont_remove_fcu_meta,
		// Token: 0x04000132 RID: 306
		tooltip_dont_remove_fcu_meta,
		// Token: 0x04000133 RID: 307
		label_download_fonts_from_project,
		// Token: 0x04000134 RID: 308
		tooltip_download_fonts_from_project,
		// Token: 0x04000135 RID: 309
		label_sprite_download_options,
		// Token: 0x04000136 RID: 310
		tooltip_sprite_download_options,
		// Token: 0x04000137 RID: 311
		label_download_unsupported_gradients,
		// Token: 0x04000138 RID: 312
		tooltip_download_unsupported_gradients,
		// Token: 0x04000139 RID: 313
		label_fade_duration,
		// Token: 0x0400013A RID: 314
		tooltip_fade_duration,
		// Token: 0x0400013B RID: 315
		label_figma_auth,
		// Token: 0x0400013C RID: 316
		tooltip_figma_auth,
		// Token: 0x0400013D RID: 317
		label_figma_color,
		// Token: 0x0400013E RID: 318
		tooltip_figma_color,
		// Token: 0x0400013F RID: 319
		label_figma_comp,
		// Token: 0x04000140 RID: 320
		tooltip_figma_comp,
		// Token: 0x04000141 RID: 321
		label_figma_comp_desc,
		// Token: 0x04000142 RID: 322
		tooltip_figma_comp_desc,
		// Token: 0x04000143 RID: 323
		label_figma_layout_culture,
		// Token: 0x04000144 RID: 324
		tooltip_figma_layout_culture,
		// Token: 0x04000145 RID: 325
		label_find_added_objects,
		// Token: 0x04000146 RID: 326
		tooltip_find_added_objects,
		// Token: 0x04000147 RID: 327
		label_flip_x,
		// Token: 0x04000148 RID: 328
		tooltip_flip_x,
		// Token: 0x04000149 RID: 329
		label_flip_y,
		// Token: 0x0400014A RID: 330
		tooltip_flip_y,
		// Token: 0x0400014B RID: 331
		label_font_settings,
		// Token: 0x0400014C RID: 332
		tooltip_font_settings,
		// Token: 0x0400014D RID: 333
		label_font_subset,
		// Token: 0x0400014E RID: 334
		tooltip_font_subset,
		// Token: 0x0400014F RID: 335
		label_force_fix,
		// Token: 0x04000150 RID: 336
		tooltip_force_fix,
		// Token: 0x04000151 RID: 337
		label_frames_to_import,
		// Token: 0x04000152 RID: 338
		tooltip_frames_to_import,
		// Token: 0x04000153 RID: 339
		label_generate_physics_shape,
		// Token: 0x04000154 RID: 340
		tooltip_generate_physics_shape,
		// Token: 0x04000155 RID: 341
		label_gradient_resolution,
		// Token: 0x04000156 RID: 342
		tooltip_gradient_resolution,
		// Token: 0x04000157 RID: 343
		label_highlighted_color,
		// Token: 0x04000158 RID: 344
		tooltip_highlighted_color,
		// Token: 0x04000159 RID: 345
		label_horizontal_mapping,
		// Token: 0x0400015A RID: 346
		tooltip_horizontal_mapping,
		// Token: 0x0400015B RID: 347
		label_horizontal_overflow,
		// Token: 0x0400015C RID: 348
		tooltip_horizontal_overflow,
		// Token: 0x0400015D RID: 349
		label_https_setting,
		// Token: 0x0400015E RID: 350
		tooltip_https_setting,
		// Token: 0x0400015F RID: 351
		label_image_and_sprites,
		// Token: 0x04000160 RID: 352
		tooltip_image_and_sprites,
		// Token: 0x04000161 RID: 353
		label_image_component,
		// Token: 0x04000162 RID: 354
		tooltip_image_component,
		// Token: 0x04000163 RID: 355
		label_image_type,
		// Token: 0x04000164 RID: 356
		tooltip_image_type,
		// Token: 0x04000165 RID: 357
		label_import,
		// Token: 0x04000166 RID: 358
		tooltip_import,
		// Token: 0x04000167 RID: 359
		label_import_events,
		// Token: 0x04000168 RID: 360
		tooltip_import_events,
		// Token: 0x04000169 RID: 361
		log_import_stoped_because_error,
		// Token: 0x0400016A RID: 362
		tooltip_import_stoped_because_error,
		// Token: 0x0400016B RID: 363
		log_import_stoped_manually,
		// Token: 0x0400016C RID: 364
		tooltip_stop_import,
		// Token: 0x0400016D RID: 365
		label_kerning,
		// Token: 0x0400016E RID: 366
		tooltip_kerning,
		// Token: 0x0400016F RID: 367
		label_kilobytes,
		// Token: 0x04000170 RID: 368
		tooltip_kilobytes,
		// Token: 0x04000171 RID: 369
		label_layout_culture,
		// Token: 0x04000172 RID: 370
		tooltip_layout_culture,
		// Token: 0x04000173 RID: 371
		label_line_spacing,
		// Token: 0x04000174 RID: 372
		tooltip_line_spacing,
		// Token: 0x04000175 RID: 373
		label_loc_case_type,
		// Token: 0x04000176 RID: 374
		tooltip_loc_case_type,
		// Token: 0x04000177 RID: 375
		label_loc_component,
		// Token: 0x04000178 RID: 376
		tooltip_loc_component,
		// Token: 0x04000179 RID: 377
		label_loc_file_name,
		// Token: 0x0400017A RID: 378
		tooltip_loc_file_name,
		// Token: 0x0400017B RID: 379
		label_loc_file_path,
		// Token: 0x0400017C RID: 380
		tooltip_loc_file_path,
		// Token: 0x0400017D RID: 381
		label_localization_settings,
		// Token: 0x0400017E RID: 382
		tooltip_localization_settings,
		// Token: 0x0400017F RID: 383
		label_localizator,
		// Token: 0x04000180 RID: 384
		tooltip_localizator,
		// Token: 0x04000181 RID: 385
		label_log_default,
		// Token: 0x04000182 RID: 386
		tooltip_log_default,
		// Token: 0x04000183 RID: 387
		label_log_downloadable,
		// Token: 0x04000184 RID: 388
		tooltip_log_downloadable,
		// Token: 0x04000185 RID: 389
		label_log_go_drawer,
		// Token: 0x04000186 RID: 390
		tooltip_log_go_drawer,
		// Token: 0x04000187 RID: 391
		label_log_set_tag,
		// Token: 0x04000188 RID: 392
		tooltip_log_set_tag,
		// Token: 0x04000189 RID: 393
		label_log_transform,
		// Token: 0x0400018A RID: 394
		tooltip_log_transform,
		// Token: 0x0400018B RID: 395
		label_made_by,
		// Token: 0x0400018C RID: 396
		tooltip_made_by,
		// Token: 0x0400018D RID: 397
		label_main_settings,
		// Token: 0x0400018E RID: 398
		tooltip_main_settings,
		// Token: 0x0400018F RID: 399
		label_mask_interaction,
		// Token: 0x04000190 RID: 400
		tooltip_mask_interaction,
		// Token: 0x04000191 RID: 401
		label_maskable,
		// Token: 0x04000192 RID: 402
		tooltip_maskable,
		// Token: 0x04000193 RID: 403
		label_max_cord_deviation_enabled,
		// Token: 0x04000194 RID: 404
		tooltip_max_cord_deviation_enabled,
		// Token: 0x04000195 RID: 405
		label_max_tangent_angle_enabled,
		// Token: 0x04000196 RID: 406
		tooltip_max_tangent_angle_enabled,
		// Token: 0x04000197 RID: 407
		label_mipmap_enabled,
		// Token: 0x04000198 RID: 408
		tooltip_mipmap_enabled,
		// Token: 0x04000199 RID: 409
		label_missings_in_frame,
		// Token: 0x0400019A RID: 410
		tooltip_missings_in_frame,
		// Token: 0x0400019B RID: 411
		label_namespace,
		// Token: 0x0400019C RID: 412
		tooltip_namespace,
		// Token: 0x0400019D RID: 413
		label_new,
		// Token: 0x0400019E RID: 414
		tooltip_new,
		// Token: 0x0400019F RID: 415
		label_no_recent_projects,
		// Token: 0x040001A0 RID: 416
		tooltip_no_recent_projects,
		// Token: 0x040001A1 RID: 417
		label_normal_color,
		// Token: 0x040001A2 RID: 418
		tooltip_normal_color,
		// Token: 0x040001A3 RID: 419
		label_old_data,
		// Token: 0x040001A4 RID: 420
		tooltip_old_data,
		// Token: 0x040001A5 RID: 421
		label_override_line_spacing_px,
		// Token: 0x040001A6 RID: 422
		tooltip_override_line_spacing_px,
		// Token: 0x040001A7 RID: 423
		label_override_tags,
		// Token: 0x040001A8 RID: 424
		tooltip_override_tags,
		// Token: 0x040001A9 RID: 425
		label_parse_escape_characters,
		// Token: 0x040001AA RID: 426
		tooltip_parse_escape_characters,
		// Token: 0x040001AB RID: 427
		label_pixels_per_unit,
		// Token: 0x040001AC RID: 428
		tooltip_pixels_per_unit,
		// Token: 0x040001AD RID: 429
		label_positioning_mode,
		// Token: 0x040001AE RID: 430
		tooltip_positioning_mode,
		// Token: 0x040001AF RID: 431
		label_prefab_creator,
		// Token: 0x040001B0 RID: 432
		tooltip_prefab_creator,
		// Token: 0x040001B1 RID: 433
		label_prefabs_path,
		// Token: 0x040001B2 RID: 434
		tooltip_prefabs_path,
		// Token: 0x040001B3 RID: 435
		label_preserve_aspect,
		// Token: 0x040001B4 RID: 436
		tooltip_preserve_aspect,
		// Token: 0x040001B5 RID: 437
		label_preserve_numbers,
		// Token: 0x040001B6 RID: 438
		tooltip_preserve_numbers,
		// Token: 0x040001B7 RID: 439
		label_pressed_color,
		// Token: 0x040001B8 RID: 440
		tooltip_pressed_color,
		// Token: 0x040001B9 RID: 441
		label_project_url,
		// Token: 0x040001BA RID: 442
		tooltip_project_url,
		// Token: 0x040001BB RID: 443
		label_pui_falloff_distance,
		// Token: 0x040001BC RID: 444
		tooltip_pui_falloff_distance,
		// Token: 0x040001BD RID: 445
		label_rate,
		// Token: 0x040001BE RID: 446
		tooltip_rate,
		// Token: 0x040001BF RID: 447
		label_rateme,
		// Token: 0x040001C0 RID: 448
		tooltip_rateme,
		// Token: 0x040001C1 RID: 449
		label_rateme_desc,
		// Token: 0x040001C2 RID: 450
		tooltip_rateme_desc,
		// Token: 0x040001C3 RID: 451
		label_raycast_padding,
		// Token: 0x040001C4 RID: 452
		tooltip_raycast_padding,
		// Token: 0x040001C5 RID: 453
		label_raycast_target,
		// Token: 0x040001C6 RID: 454
		tooltip_raycast_target,
		// Token: 0x040001C7 RID: 455
		label_redownload_sprites,
		// Token: 0x040001C8 RID: 456
		tooltip_redownload_sprites,
		// Token: 0x040001C9 RID: 457
		label_remove,
		// Token: 0x040001CA RID: 458
		tooltip_remove,
		// Token: 0x040001CB RID: 459
		label_remove_unused_sprites,
		// Token: 0x040001CC RID: 460
		tooltip_remove_unused_sprites,
		// Token: 0x040001CD RID: 461
		label_render_mode,
		// Token: 0x040001CE RID: 462
		tooltip_render_mode,
		// Token: 0x040001CF RID: 463
		label_rich_text,
		// Token: 0x040001D0 RID: 464
		tooltip_rich_text,
		// Token: 0x040001D1 RID: 465
		label_rtl_textmeshpro_settings,
		// Token: 0x040001D2 RID: 466
		tooltip_rtl_textmeshpro_settings,
		// Token: 0x040001D3 RID: 467
		label_sampling_steps,
		// Token: 0x040001D4 RID: 468
		tooltip_sampling_steps,
		// Token: 0x040001D5 RID: 469
		label_selected_color,
		// Token: 0x040001D6 RID: 470
		tooltip_selected_color,
		// Token: 0x040001D7 RID: 471
		label_shadow_type,
		// Token: 0x040001D8 RID: 472
		tooltip_shadow_type,
		// Token: 0x040001D9 RID: 473
		label_shadows_tab,
		// Token: 0x040001DA RID: 474
		tooltip_shadows_tab,
		// Token: 0x040001DB RID: 475
		label_sort_point,
		// Token: 0x040001DC RID: 476
		tooltip_sort_point,
		// Token: 0x040001DD RID: 477
		label_sorting_layer,
		// Token: 0x040001DE RID: 478
		tooltip_sorting_layer,
		// Token: 0x040001DF RID: 479
		label_sprite_import_mode,
		// Token: 0x040001E0 RID: 480
		tooltip_sprite_import_mode,
		// Token: 0x040001E1 RID: 481
		label_step_distance,
		// Token: 0x040001E2 RID: 482
		tooltip_step_distance,
		// Token: 0x040001E3 RID: 483
		label_svg_image_settings,
		// Token: 0x040001E4 RID: 484
		tooltip_svg_image_settings,
		// Token: 0x040001E5 RID: 485
		label_svg_importer_settings,
		// Token: 0x040001E6 RID: 486
		tooltip_svg_importer_settings,
		// Token: 0x040001E7 RID: 487
		label_svg_type,
		// Token: 0x040001E8 RID: 488
		tooltip_svg_type,
		// Token: 0x040001E9 RID: 489
		label_text_and_fonts,
		// Token: 0x040001EA RID: 490
		tooltip_text_and_fonts,
		// Token: 0x040001EB RID: 491
		label_text_and_font_settings,
		// Token: 0x040001EC RID: 492
		tooltip_text_and_font_settings,
		// Token: 0x040001ED RID: 493
		label_text_component,
		// Token: 0x040001EE RID: 494
		tooltip_text_component,
		// Token: 0x040001EF RID: 495
		label_textmeshpro_settings,
		// Token: 0x040001F0 RID: 496
		tooltip_textmeshpro_settings,
		// Token: 0x040001F1 RID: 497
		label_texture_compression,
		// Token: 0x040001F2 RID: 498
		tooltip_texture_compression,
		// Token: 0x040001F3 RID: 499
		label_texture_importer_settings,
		// Token: 0x040001F4 RID: 500
		tooltip_texture_importer_settings,
		// Token: 0x040001F5 RID: 501
		label_texture_type,
		// Token: 0x040001F6 RID: 502
		tooltip_texture_type,
		// Token: 0x040001F7 RID: 503
		label_ttf_path,
		// Token: 0x040001F8 RID: 504
		tooltip_ttf_path,
		// Token: 0x040001F9 RID: 505
		label_ui_framework,
		// Token: 0x040001FA RID: 506
		tooltip_ui_framework,
		// Token: 0x040001FB RID: 507
		label_ui_toolkit,
		// Token: 0x040001FC RID: 508
		tooltip_ui_toolkit,
		// Token: 0x040001FD RID: 509
		label_unity_comp,
		// Token: 0x040001FE RID: 510
		tooltip_unity_comp,
		// Token: 0x040001FF RID: 511
		label_unity_image_settings,
		// Token: 0x04000200 RID: 512
		tooltip_unity_image_settings,
		// Token: 0x04000201 RID: 513
		label_unity_text_settings,
		// Token: 0x04000202 RID: 514
		tooltip_unity_text_settings,
		// Token: 0x04000203 RID: 515
		label_uitk_linking_mode,
		// Token: 0x04000204 RID: 516
		tooltip_uitk_linking_mode,
		// Token: 0x04000205 RID: 517
		label_uitk_output_path,
		// Token: 0x04000206 RID: 518
		tooltip_uitk_output_path,
		// Token: 0x04000207 RID: 519
		label_use_i2localization,
		// Token: 0x04000208 RID: 520
		tooltip_use_i2localization,
		// Token: 0x04000209 RID: 521
		label_visible_descender,
		// Token: 0x0400020A RID: 522
		tooltip_visible_descender,
		// Token: 0x0400020B RID: 523
		label_viewport_options,
		// Token: 0x0400020C RID: 524
		tooltip_viewport_options,
		// Token: 0x0400020D RID: 525
		label_vertical_mapping,
		// Token: 0x0400020E RID: 526
		tooltip_vertical_mapping,
		// Token: 0x0400020F RID: 527
		label_vertical_overflow,
		// Token: 0x04000210 RID: 528
		tooltip_vertical_overflow,
		// Token: 0x04000211 RID: 529
		label_without_changes,
		// Token: 0x04000212 RID: 530
		tooltip_label_without_changes,
		// Token: 0x04000213 RID: 531
		label_wrapping,
		// Token: 0x04000214 RID: 532
		tooltip_wrapping,
		// Token: 0x04000215 RID: 533
		label_xml_parsing,
		// Token: 0x04000216 RID: 534
		tooltip_xml_parsing,
		// Token: 0x04000217 RID: 535
		label_procedural_ui_settings,
		// Token: 0x04000218 RID: 536
		tooltip_procedural_ui_settings,
		// Token: 0x04000219 RID: 537
		label_pui_settings,
		// Token: 0x0400021A RID: 538
		tooltip_pui_settings,
		// Token: 0x0400021B RID: 539
		label_color_multiplier,
		// Token: 0x0400021C RID: 540
		tooltip_color_multiplier,
		// Token: 0x0400021D RID: 541
		label_mpuikit_settings,
		// Token: 0x0400021E RID: 542
		tooltip_mpuikit_settings,
		// Token: 0x0400021F RID: 543
		label_sr_settings,
		// Token: 0x04000220 RID: 544
		tooltip_sr_settings,
		// Token: 0x04000221 RID: 545
		label_is_readable,
		// Token: 0x04000222 RID: 546
		tooltip_is_readable,
		// Token: 0x04000223 RID: 547
		label_next_order_step,
		// Token: 0x04000224 RID: 548
		tooltip_next_order_step,
		// Token: 0x04000225 RID: 549
		label_raw_import,
		// Token: 0x04000226 RID: 550
		tooltip_raw_import,
		// Token: 0x04000227 RID: 551
		label_images_and_sprites_tab,
		// Token: 0x04000228 RID: 552
		tooltip_images_and_sprites_tab,
		// Token: 0x04000229 RID: 553
		label_buttons_tab,
		// Token: 0x0400022A RID: 554
		tooltip_buttons_tab,
		// Token: 0x0400022B RID: 555
		label_ui_toolkit_tab,
		// Token: 0x0400022C RID: 556
		tooltip_ui_toolkit_tab,
		// Token: 0x0400022D RID: 557
		label_nova_components,
		// Token: 0x0400022E RID: 558
		tooltip_nova_components,
		// Token: 0x0400022F RID: 559
		label_fcu,
		// Token: 0x04000230 RID: 560
		tooltip_fcu,
		// Token: 0x04000231 RID: 561
		label_settings,
		// Token: 0x04000232 RID: 562
		tooltip_settings,
		// Token: 0x04000233 RID: 563
		label_script_generator,
		// Token: 0x04000234 RID: 564
		tooltip_script_generator,
		// Token: 0x04000235 RID: 565
		label_enabled,
		// Token: 0x04000236 RID: 566
		tooltip_enabled,
		// Token: 0x04000237 RID: 567
		label_scripts_output_path,
		// Token: 0x04000238 RID: 568
		tooltip_scripts_output_path,
		// Token: 0x04000239 RID: 569
		label_select_folder,
		// Token: 0x0400023A RID: 570
		tooltip_select_folder,
		// Token: 0x0400023B RID: 571
		label_text_prefab_naming_mode,
		// Token: 0x0400023C RID: 572
		tooltip_text_prefab_naming_mode,
		// Token: 0x0400023D RID: 573
		label_humanized_color,
		// Token: 0x0400023E RID: 574
		tooltip_humanized_color,
		// Token: 0x0400023F RID: 575
		label_hex_color,
		// Token: 0x04000240 RID: 576
		tooltip_hex_color,
		// Token: 0x04000241 RID: 577
		label_log_component_drawer,
		// Token: 0x04000242 RID: 578
		tooltip_log_component_drawer,
		// Token: 0x04000243 RID: 579
		label_log_hash_generator_drawer,
		// Token: 0x04000244 RID: 580
		tooltip_log_hash_generator_drawer,
		// Token: 0x04000245 RID: 581
		label_override_tmp_letter_spacing,
		// Token: 0x04000246 RID: 582
		tooltip_override_tmp_letter_spacing,
		// Token: 0x04000247 RID: 583
		label_object_comparer,
		// Token: 0x04000248 RID: 584
		tooltip_object_comparer,
		// Token: 0x04000249 RID: 585
		label_fcu_is_null,
		// Token: 0x0400024A RID: 586
		tooltip_fcu_is_null,
		// Token: 0x0400024B RID: 587
		label_more_about_layout_updating,
		// Token: 0x0400024C RID: 588
		tooltip_more_about_layout_updating,
		// Token: 0x0400024D RID: 589
		tooltip_open_fcu_window,
		// Token: 0x0400024E RID: 590
		tooltip_change_window_mode,
		// Token: 0x0400024F RID: 591
		label_orthographic_mode,
		// Token: 0x04000250 RID: 592
		tooltip_orthographic_mode,
		// Token: 0x04000251 RID: 593
		label_extra_padding,
		// Token: 0x04000252 RID: 594
		tooltip_extra_padding,
		// Token: 0x04000253 RID: 595
		label_overflow,
		// Token: 0x04000254 RID: 596
		tooltip_overflow,
		// Token: 0x04000255 RID: 597
		label_geometry_sorting,
		// Token: 0x04000256 RID: 598
		tooltip_geometry_sorting,
		// Token: 0x04000257 RID: 599
		label_shader,
		// Token: 0x04000258 RID: 600
		tooltip_shader,
		// Token: 0x04000259 RID: 601
		label_farsi,
		// Token: 0x0400025A RID: 602
		tooltip_farsi,
		// Token: 0x0400025B RID: 603
		label_fix_tags,
		// Token: 0x0400025C RID: 604
		tooltip_fix_tags,
		// Token: 0x0400025D RID: 605
		label_user_name,
		// Token: 0x0400025E RID: 606
		tooltip_user_id,
		// Token: 0x0400025F RID: 607
		label_asset_instance_id,
		// Token: 0x04000260 RID: 608
		tooltip_asset_instance_id,
		// Token: 0x04000261 RID: 609
		label_stable_version,
		// Token: 0x04000262 RID: 610
		tooltip_stable_version,
		// Token: 0x04000263 RID: 611
		label_open_diff_checker,
		// Token: 0x04000264 RID: 612
		tooltip_open_diff_checker,
		// Token: 0x04000265 RID: 613
		label_go_name_max_length,
		// Token: 0x04000266 RID: 614
		tooltip_go_name_max_length,
		// Token: 0x04000267 RID: 615
		label_text_name_max_length,
		// Token: 0x04000268 RID: 616
		tooltip_text_name_max_length,
		// Token: 0x04000269 RID: 617
		label_go_layer,
		// Token: 0x0400026A RID: 618
		tooltip_go_layer,
		// Token: 0x0400026B RID: 619
		label_pivot_type,
		// Token: 0x0400026C RID: 620
		tooltip_pivot_type,
		// Token: 0x0400026D RID: 621
		label_components_to_import,
		// Token: 0x0400026E RID: 622
		tooltip_components_to_import,
		// Token: 0x0400026F RID: 623
		label_components_with_count,
		// Token: 0x04000270 RID: 624
		tooltip_components_with_count,
		// Token: 0x04000271 RID: 625
		tooltip_recent_projects,
		// Token: 0x04000272 RID: 626
		tooltip_download_project,
		// Token: 0x04000273 RID: 627
		label_sampling_point_size,
		// Token: 0x04000274 RID: 628
		tooltip_sampling_point_size,
		// Token: 0x04000275 RID: 629
		label_enable_multi_atlas_support,
		// Token: 0x04000276 RID: 630
		tooltip_enable_multi_atlas_support,
		// Token: 0x04000277 RID: 631
		label_images_format,
		// Token: 0x04000278 RID: 632
		tooltip_images_format,
		// Token: 0x04000279 RID: 633
		label_images_scale,
		// Token: 0x0400027A RID: 634
		tooltip_images_scale,
		// Token: 0x0400027B RID: 635
		label_preserve_ratio_mode,
		// Token: 0x0400027C RID: 636
		tooltip_preserve_ratio_mode,
		// Token: 0x0400027D RID: 637
		label_sprites_path,
		// Token: 0x0400027E RID: 638
		tooltip_sprites_path,
		// Token: 0x0400027F RID: 639
		label_shapes2d_settings,
		// Token: 0x04000280 RID: 640
		tooltip_shapes2d_settings,
		// Token: 0x04000281 RID: 641
		label_google_fonts_settings,
		// Token: 0x04000282 RID: 642
		tooltip_google_fonts_settings,
		// Token: 0x04000283 RID: 643
		label_google_fonts_api_key,
		// Token: 0x04000284 RID: 644
		tooltip_google_fonts_api_key,
		// Token: 0x04000285 RID: 645
		label_get_google_api_key,
		// Token: 0x04000286 RID: 646
		tooltip_get_google_api_key,
		// Token: 0x04000287 RID: 647
		label_remove_from_scene,
		// Token: 0x04000288 RID: 648
		tooltip_remove_from_scene,
		// Token: 0x04000289 RID: 649
		label_token,
		// Token: 0x0400028A RID: 650
		tooltip_token,
		// Token: 0x0400028B RID: 651
		tooltip_recent_tokens,
		// Token: 0x0400028C RID: 652
		tooltip_auth,
		// Token: 0x0400028D RID: 653
		label_tmp_path,
		// Token: 0x0400028E RID: 654
		tooltip_tmp_path,
		// Token: 0x0400028F RID: 655
		label_no_recent_sessions,
		// Token: 0x04000290 RID: 656
		tooltip_no_recent_sessions,
		// Token: 0x04000291 RID: 657
		label_different_component_data,
		// Token: 0x04000292 RID: 658
		tooltip_different_component_data,
		// Token: 0x04000293 RID: 659
		label_has_differences,
		// Token: 0x04000294 RID: 660
		tooltip_has_differences,
		// Token: 0x04000295 RID: 661
		label_new_components,
		// Token: 0x04000296 RID: 662
		tooltip_new_components,
		// Token: 0x04000297 RID: 663
		label_import_frames,
		// Token: 0x04000298 RID: 664
		tooltip_import_frames,
		// Token: 0x04000299 RID: 665
		label_open_settings_window,
		// Token: 0x0400029A RID: 666
		tooltip_open_settings_window,
		// Token: 0x0400029B RID: 667
		label_select_fonts_folder,
		// Token: 0x0400029C RID: 668
		tooltip_select_fonts_folder,
		// Token: 0x0400029D RID: 669
		label_select_prefabs_folder,
		// Token: 0x0400029E RID: 670
		tooltip_select_prefabs_folder,
		// Token: 0x0400029F RID: 671
		label_max_cord_deviation,
		// Token: 0x040002A0 RID: 672
		tooltip_max_cord_deviation,
		// Token: 0x040002A1 RID: 673
		label_max_tangent_angle,
		// Token: 0x040002A2 RID: 674
		tooltip_max_tangent_angle,
		// Token: 0x040002A3 RID: 675
		label_loc_folder_path,
		// Token: 0x040002A4 RID: 676
		tooltip_loc_folder_path,
		// Token: 0x040002A5 RID: 677
		label_use_image_linear_material,
		// Token: 0x040002A6 RID: 678
		tooltip_use_image_linear_material,
		// Token: 0x040002A7 RID: 679
		label_loc_key_max_lenght,
		// Token: 0x040002A8 RID: 680
		tooltip_loc_key_max_lenght,
		// Token: 0x040002A9 RID: 681
		label_supported_from_unity_version,
		// Token: 0x040002AA RID: 682
		label_field_name_max_length,
		// Token: 0x040002AB RID: 683
		tooltip_field_name_max_length,
		// Token: 0x040002AC RID: 684
		label_method_name_max_length,
		// Token: 0x040002AD RID: 685
		tooltip_method_name_max_length,
		// Token: 0x040002AE RID: 686
		label_class_name_max_length,
		// Token: 0x040002AF RID: 687
		tooltip_class_name_max_length,
		// Token: 0x040002B0 RID: 688
		label_base_class,
		// Token: 0x040002B1 RID: 689
		tooltip_base_class,
		// Token: 0x040002B2 RID: 690
		label_serialization_mode,
		// Token: 0x040002B3 RID: 691
		tooltip_serialization_mode,
		// Token: 0x040002B4 RID: 692
		label_max_sprite_size,
		// Token: 0x040002B5 RID: 693
		tooltip_max_sprite_size,
		// Token: 0x040002B6 RID: 694
		label_transition_type,
		// Token: 0x040002B7 RID: 695
		tooltip_transition_type
	}
}
